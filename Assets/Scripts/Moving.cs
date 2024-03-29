using UnityEngine;
using System.Collections;
using InControl;

public class Moving : MonoBehaviour 
{
	public enum Direction
	{
		left, right, up, down
	};
	
	public InputDevice playerController;
	public PlayerManager manager;
	public Player myPlayer;
	public Vector3 SpawnPoint;	
	
	public LayerMask FloorMask;
	
	#region xMovement Speeds
	private float xInput;
	private float xMovement;
	private float accelRate = 1.3f;
	private float maxSpeed = 9f;
	private float drag = 0.87f;
	private float reductionAmount;
	private float minimumReduction = 0.4f;
	
	private float airDrag = 0.96f;
	private float airAccel = 0.8f;
	#endregion
	
	#region yMovement stuff
	private float gravityForceHigh = 7f;
	private float gravityCutoffHigh = 8f;
	private float gravityForceMed = 1f;
	private float gravityCutoffMed = -0.1f;
	private float gravityForceLow = 0.9f;
	private float gravityMax = -14f;
	private float jumpSpeed = 11f;
	private float holdJumpReduction = 0.5f;
	private float ySpeed;
	
	private float jumpHoldTime;
	private float jumpHoldLength = 0.3f;
	
	private bool jumpHold = false;
	
	private RaycastHit2D hitInfo;
	private bool grounded = false;
	
	private float tempNoLand;
	private float noLandLength = 0.14f;
	#endregion

	#region Platform stuff
	private Vector2 platformVelocity;
	private GameObject platform;
	private bool onMovingPlatform;
	#endregion
	
	#region Slide and Wall Jump
	private bool sliding = false;
	private bool slideRight = false;
	private float slideSpeed = -3f;
	private float slideAccel = 0.3f;
	private float slideJumpSpeed = 6.5f;

	private bool slideRelease = false;
	private float timeFromRelease;
	private float lengthForRelease = 0.1f;
	#endregion
	
	#region Raycast Origins
	private Vector2 upperLeft = new Vector2(-0.054f , -0.12f);
	private Vector2 upperRight = new Vector2(0.054f , -0.12f);
	private Vector2 midLeft = new Vector2 (-0.054f, 0f);
	private Vector2 midRight = new Vector2 (0.054f, 0f);
	private Vector2 lowerLeft = new Vector2(-0.054f, -0.18f);
	private Vector2 lowerRight = new Vector2(0.054f, -0.18f);
	#endregion
	
	#region Attack
	public GameObject attackHitBox;
	private float attackCD = 0.5f;
	private float attackCDTimer;
	private float attackTime = 0.16f;
	private float attackTimer;
	
	private Vector2 horizontalPosition = Vector2.right * 0.03f;
	private Vector2 verticlePosition = Vector2.up * 0.01f;
	private Vector3 rotation = Vector3.forward * 90f;
	
	private float yInput;
	#endregion
	
	#region Trap
	public GameObject stunTrapPrefab;
	public GameObject prevTrap;
	private float trapCD = 4f;
	private float trapCDTimer;
	private Vector2 trapPoint;
	
	private float trappedTime;
	private float trapLength = 1f;
	private bool trapped = false;
	#endregion
	
	#region Hit
	private bool Hit = false;
	private Direction hitDirection;
	public LayerMask hitCollideDetection;
	
	private Vector2 topRecoverCheck = new Vector2(0f, 0.18f);
	private Vector2 botRecoverCheck = new Vector2(0f, -0.18f);
	
	private bool recovered = false;
	private float recoverTimer;
	private float revoverLength = 0.1f;
	#endregion
	
	public GameObject throughCollider;
	public GameObject DeathParticles;
	private GameObject deathParticleHolder;
	
	private Vector2 Velocity;
	
	void Start()
	{
		attackHitBox.transform.localPosition = horizontalPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!myPlayer.dead)
		{
			if(!Hit && !recovered && !trapped)
			{
				#region Get Movement Input
				xInput = 0;
				xInput = playerController.LeftStickX;
				if(Mathf.Abs(xInput) < 0.1f) xInput = 0;
				
				if(playerController.DPadRight || playerController.DPadLeft)
				{
					xInput = 0;
					if(playerController.DPadRight)	xInput += 1;
					if(playerController.DPadLeft) xInput -= 1;
				}
				
				yInput = 0;
				yInput = playerController.LeftStickY;
				
				if(playerController.DPadUp || playerController.DPadDown)
				{
					yInput = 0;
					if(playerController.DPadUp)	yInput += 1;
					if(playerController.DPadDown) yInput -= 1;
				}
				#endregion
				
				#region xMovement
				if(grounded)
				{
					//Applying the users input here
					xMovement += (xInput * accelRate);
					if(Mathf.Abs(xMovement) > maxSpeed)
					{
						if(xMovement > 0) xMovement = maxSpeed;
						else xMovement = -maxSpeed;
					}
					
					//Here we give the Velocity vector our x movement
					Velocity.x = xMovement;
					
					//Then we apply our reduction
					reductionAmount = xMovement * drag;
					reductionAmount = Mathf.Abs(xMovement - reductionAmount);
					if(reductionAmount < minimumReduction)
					{
						if(Mathf.Abs(xMovement) > minimumReduction)
						{
							if(xMovement > 0)	xMovement -= minimumReduction;
							else xMovement += minimumReduction;
						}
						else xMovement = 0;
					}
					else xMovement *= drag;
				}
				else
				{
					//Applying the users input here
					xMovement += (xInput * accelRate * 0.5f);
					if(Mathf.Abs(xMovement) > maxSpeed)
					{
						if(xMovement > 0) xMovement = maxSpeed;
						else xMovement = -maxSpeed;
					}
					//Here we give the Velocity vector our x movement
					Velocity.x = xMovement;
					//Then we apply our reduction
					xMovement *= airDrag;
				}
				#endregion
				
				#region yMovement
				if(!grounded)
				{
					if(!sliding)
					{
						Gravity ();
						

						if(xInput >= 0.5f)
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + upperRight, Vector2.right, 0.03f, hitCollideDetection);
							if(NoWallSlide())
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + midRight, Vector2.right, 0.03f, hitCollideDetection);
								if(NoWallSlide ())
								{
									hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerRight, Vector2.right, 0.03f, hitCollideDetection);
									NoWallSlide();
								}
							}
							
							slideRight = true;
							
						}
						else if(xInput <= -0.5f)
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + upperLeft, -Vector2.right, 0.03f, hitCollideDetection);
							if(NoWallSlide())
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + midLeft, -Vector2.right, 0.03f, hitCollideDetection);
								if(NoWallSlide())
								{
									hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerLeft, -Vector2.right, 0.03f, hitCollideDetection);
									NoWallSlide ();
								}
							}
							slideRight = false;
						}
					}
					else if(sliding)
					{
						if(ySpeed < 0)
						{
							if (ySpeed > slideSpeed)
							{
								ySpeed -= slideAccel;
								if(ySpeed < slideSpeed) ySpeed = slideSpeed;
							}
							else if(ySpeed < slideSpeed)
							{
								ySpeed += slideAccel;
								if(ySpeed > slideSpeed) ySpeed = slideSpeed;
							}
						}
						else
						{
							Gravity();
						}
						
						Velocity.y = ySpeed;
						
						#region Detect move away

						if(slideRight)
						{
							if(!slideRelease && xInput < 0.4f) 
							{
								timeFromRelease = Time.time + lengthForRelease;
								slideRelease = true;
							}
							else if(slideRelease && xInput > 0.4f) slideRelease = false;

							hitInfo = Physics2D.Raycast((Vector2)transform.position + upperRight, Vector2.right, 0.03f, hitCollideDetection);
							if(hitInfo.transform == null)
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + midRight, Vector2.right, 0.03f, hitCollideDetection);
								if(hitInfo.transform == null)
								{
									hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerRight, Vector2.right, 0.03f, hitCollideDetection);
									if(hitInfo.transform == null)
									{
										sliding = false;
									}
								}
							}
							
						}
						else
						{

							if(!slideRelease && xInput > -0.4f) 
							{
								timeFromRelease = Time.time + lengthForRelease;
								slideRelease = true;
							}
							else if(slideRelease && xInput < -0.4f) slideRelease = false;

							hitInfo = Physics2D.Raycast((Vector2)transform.position + upperLeft, -Vector2.right, 0.03f, hitCollideDetection);
							if(hitInfo.transform == null)
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + midLeft, -Vector2.right, 0.03f, hitCollideDetection);
								if(hitInfo.transform == null)
								{
									hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerLeft, -Vector2.right, 0.03f, hitCollideDetection);
									if(hitInfo.transform == null)
									{

										sliding = false;
									}
								}
							}
						}

						if(slideRelease && timeFromRelease < Time.time)
						{
							sliding = false;
							slideRelease = false;
							if(slideRight) xMovement = -0.2f;
							else xMovement = 0.2f;
						}
						#endregion
					}
				}
				#endregion
			}
			else if(Hit)
			{
				#region D-Influence while hit
				switch(hitDirection)
				{
				case Direction.up:
				case Direction.down:
					xMovement += (yInput * 0.1f);
					xMovement = Mathf.Clamp(xMovement, -3f, 3f);
					
					Velocity.x = xMovement;
					//rigidbody2D.velocity = Velocity;
					break;
				case Direction.left:
				case Direction.right:
					ySpeed += (yInput * 0.1f);
					ySpeed = Mathf.Clamp(ySpeed, -3f, 3f);
					
					Velocity.y = ySpeed;
					//rigidbody2D.velocity = Velocity;
					break;
				}
				#endregion
			}
			
			
			#region CheckForDeath
			if(transform.position.x > manager.rightKill)
			{
				HandleDeath();
				if(deathParticleHolder.activeSelf) deathParticleHolder.transform.eulerAngles = new Vector3(180, 90, 0);
			}
			else if(transform.position.x < manager.leftKill)
			{
				HandleDeath();
			}
			else if(transform.position.y < manager.botKill)
			{
				HandleDeath();
				if(deathParticleHolder.activeSelf) 
				{
					deathParticleHolder.transform.eulerAngles = new Vector3(270, 90, 0);
				}
			}
			else if(transform.position.y > manager.topKill)
			{
				HandleDeath();
				if(deathParticleHolder.activeSelf) deathParticleHolder.transform.eulerAngles = new Vector3(90, 90, 0);
			}
			#endregion
		}
		
		
		rigidbody2D.velocity = Velocity;
		if(onMovingPlatform)
		{
			platformVelocity = platform.rigidbody2D.velocity;
			rigidbody2D.velocity = platformVelocity + Velocity;
		}
		else if(platformVelocity != Vector2.zero)
		{
			Velocity += platformVelocity;
			platformVelocity = Vector2.zero;

			xMovement = Velocity.x;
			ySpeed = Velocity.y;
			rigidbody2D.velocity = Velocity;
		}

	}
	
	void Update()
	{
		#region Jumping
		if(!myPlayer.dead)
		{
			if(!trapped && !Hit)
			{
				if(grounded)
				{
					if(playerController.Action1.WasPressed)
					{
						hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerLeft, -Vector2.up, 0.2f, FloorMask);
						if (!CheckForJumpThrough ()) 
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerRight, -Vector2.up, 0.2f, FloorMask);
							if(!CheckForJumpThrough()) jump ();
						}
					}
				}

				if(sliding && playerController.Action1.WasPressed)
				{
					jump();
					if(slideRight)
					{
						xMovement = -slideJumpSpeed;
						Velocity.x = xMovement;
					}
					else
					{
						xMovement = slideJumpSpeed;
						Velocity.x = xMovement;
					}
				}
				#endregion
				
				#region DetectIfGrounded
				if(!grounded && ySpeed <= 0 && tempNoLand < Time.time)
				{
					
					hitInfo = Physics2D.Raycast((Vector2)transform.position, -Vector2.up, 0.2f, FloorMask);
					if(hitInfo.transform != null && (hitInfo.transform.tag == "Floor" || hitInfo.transform.tag == "MovingFloor"))
					{
						if(LayerMask.LayerToName(hitInfo.transform.gameObject.layer) != "ThroughFloor" || yInput >= -0.85f)
						{
							grounded = true;
							sliding = false;
							if(xInput == 0)	xMovement = 0;
							throughCollider.SetActive(true);

							if(hitInfo.transform.tag == "MovingFloor")
							{
								platform = hitInfo.transform.gameObject;
								onMovingPlatform = true;
							}
						}
					}
					else
					{
						hitInfo = Physics2D.Raycast((Vector2)transform.position, -Vector2.up, 0.6f, FloorMask);
						if(hitInfo.transform != null && LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == "ThroughFloor" && yInput >= -0.85f)
						{
							throughCollider.SetActive(true);
						}
					}					
				}
				else if(grounded)
				{
					hitInfo = Physics2D.Raycast ((Vector2)transform.position + lowerLeft, -Vector2.up, 0.1f, FloorMask);
					if(hitInfo.transform == null)
					{
						hitInfo = Physics2D.Raycast ((Vector2)transform.position + lowerRight, -Vector2.up, 0.1f, FloorMask);
						if(hitInfo.transform == null)
						{
							ySpeed = -0.13f;
							grounded = false;
							onMovingPlatform = false;
							throughCollider.SetActive (false);
							
							
						}
					}					
				}
				#endregion
				
				#region Laying down traps
				if(grounded && trapCDTimer < Time.time && playerController.LeftTrigger.WasPressed)
				{
					hitInfo = Physics2D.Raycast ((Vector2)transform.position, -Vector2.up, 0.2f, FloorMask);
					trapPoint = hitInfo.point;
					GameObject.Instantiate(stunTrapPrefab, trapPoint, Quaternion.identity);
					trapCDTimer = Time.time + trapCD;
					attackCDTimer = Time.time + attackCD;
				}
				#endregion
				
				#region Attacking
				if(attackCDTimer < Time.time && playerController.RightTrigger.WasPressed)
				{
					attackCDTimer = attackCD + Time.time;
					attackTimer = attackTime + Time.time;
					
					SwitchAttackHitBox(true);
					
					if(Mathf.Abs (yInput) + 0.2f > Mathf.Abs (xInput))
					{
						if(yInput > 0)
						{
							attackHitBox.transform.localPosition = verticlePosition;
							ySpeed += 4f;
						}
						else if(!grounded)
						{
							attackHitBox.transform.localPosition = -verticlePosition;
							if(ySpeed <= 0f)
							{
								ySpeed += 6f;
							}
							
						}
					}
					else if(xInput != 0)
					{
						if(xInput > 0)
						{
							attackHitBox.transform.localPosition = horizontalPosition;
							xMovement += 3f;
						}
						else 
						{
							attackHitBox.transform.localPosition = -horizontalPosition;
							xMovement -= 3f;
						}
					}
					else
					{
						if((Vector2)attackHitBox.transform.localPosition == horizontalPosition)
						{
							xMovement += 3f;
						}
						else if((Vector2)attackHitBox.transform.localPosition == -horizontalPosition)
						{
							xMovement -= 3f;
						}
					}
				}	
			}
			else if(trappedTime < Time.time)
			{
				trapped = false;
			}
			
			if(attackTimer < Time.time)
			{
				if(attackHitBox.activeSelf == true) SwitchAttackHitBox(false);
			}
			#endregion
			
			#region While hit
			if(Hit)
			{
				if(!recovered)
				{
					Velocity = Velocity.normalized * (Velocity.magnitude + 0.1f);
					
					#region CheckForRecover
					switch(hitDirection)
					{
					case Direction.right:
						hitInfo = Physics2D.Raycast((Vector2)transform.position + upperRight, Vector2.right, 0.08f, hitCollideDetection);
						if(NoRecover ())
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + midRight, Vector2.right, 0.08f, hitCollideDetection);
							if(NoRecover())
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerRight, Vector2.right, 0.08f, hitCollideDetection);
								NoRecover ();
							}
						}
						break;
					case Direction.left:
						hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerLeft, -Vector2.right, 0.08f, hitCollideDetection);
						if(NoRecover ())
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + midLeft, -Vector2.right, 0.08f, hitCollideDetection);
							if(NoRecover ())
							{
								hitInfo = Physics2D.Raycast((Vector2)transform.position + upperLeft, -Vector2.right, 0.08f, hitCollideDetection);
								NoRecover ();
							}
						}
						break;
					case Direction.down:
						hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerLeft, -Vector2.up, 0.09f, hitCollideDetection);
						if(NoRecover ())
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + lowerRight, -Vector2.up, 0.09f, hitCollideDetection);
							NoRecover ();
						}
						break;
					case Direction.up:
						hitInfo = Physics2D.Raycast((Vector2)transform.position + upperLeft, Vector2.up, 0.09f, hitCollideDetection);
						if(NoRecover ())
						{
							hitInfo = Physics2D.Raycast((Vector2)transform.position + upperRight, Vector2.up, 0.09f, hitCollideDetection);
							NoRecover ();
						}
						break;
					}
					#endregion
				}
			}
			#endregion
			#region Recover
			else if(recovered)
			{
				if(recoverTimer < Time.time)
				{
					recovered = false;
				}
				
				if(playerController.Action1.WasPressed)
				{
					recovered = false;
					jump ();
				}
			}
			#endregion
		}
	}

	private void Gravity()
	{
		if(jumpHold && playerController.Action1.IsPressed)
		{
			if(jumpHoldTime < Time.time)
			{
				jumpHold = false;
			}
			else ySpeed -= holdJumpReduction;
			
			if(ySpeed < gravityMax) ySpeed = gravityMax;
			Velocity.y = ySpeed;
		}
		else
		{
			jumpHold = false;
			if(ySpeed > gravityCutoffHigh) ySpeed -= gravityForceHigh;
			else if(ySpeed > gravityCutoffMed) ySpeed -= gravityForceMed;
			else ySpeed -= gravityForceLow;
			
			if(ySpeed < gravityMax) ySpeed = gravityMax;
			Velocity.y = ySpeed;
		}
	}
	
	private void SwitchAttackHitBox(bool _value)
	{
		if(_value) attackHitBox.SetActive(_value);
		foreach (Transform child in attackHitBox.transform)
		{
			child.gameObject.SetActive(_value);
		}
		
		if(!_value) attackHitBox.SetActive(_value);
	}
	
	private bool CheckForJumpThrough()
	{
		if(hitInfo.transform != null && LayerMask.LayerToName(hitInfo.transform.gameObject.layer) == "ThroughFloor")
		{
			if(yInput < -0.85f)
			{				
				grounded = false;
				onMovingPlatform = false;
				tempNoLand = noLandLength + Time.time;
				
				throughCollider.SetActive(false);
				return true;
			}
		}
		return false;
	}
	
	private void jump()
	{
		if(ySpeed < 0) ySpeed = jumpSpeed;
		else ySpeed += jumpSpeed;
		sliding = false;
		grounded = false;
		onMovingPlatform = false;
		jumpHold = true;
		throughCollider.SetActive (false);
		jumpHoldTime = jumpHoldLength + Time.time;
	}
	
	private bool NoRecover()
	{
		if(hitInfo.transform != null)
		{
			Hit = false;
			recovered = true;
			recoverTimer = revoverLength + Time.time;
			return false;
		}
		else return true;
	}
	
	private bool NoWallSlide()
	{
		if(hitInfo.transform != null)
		{
			sliding = true;
			return false;
		}
		else return true;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "LaunchAttack" && !Hit)
		{
			if(other.gameObject != attackHitBox)
			{
				Hit = true;
				SwitchAttackHitBox(false);
				throughCollider.SetActive(false);
				ySpeed = 0;
				xMovement = 0;
				
				if(other.transform.localPosition.x > 0 && other.transform.localPosition.x > Mathf.Abs(other.transform.localPosition.y))
				{
					hitDirection = Direction.right;
					Velocity = Vector2.right * 15f;
					//rigidbody2D.velocity = Velocity;
				}
				else if(other.transform.localPosition.x < 0 && Mathf.Abs(other.transform.localPosition.x) > Mathf.Abs(other.transform.localPosition.y))
				{
					hitDirection = Direction.left;
					Velocity = -Vector2.right * 15f;
					//rigidbody2D.velocity = Velocity;
				}
				else if(other.transform.localPosition.y > 0)
				{
					hitDirection = Direction.up;
					Velocity = Vector2.up * 15f;
					//rigidbody2D.velocity = Velocity;
				}
				else
				{
					hitDirection = Direction.down;
					Velocity = -Vector2.up * 15f;
					//rigidbody2D.velocity = Velocity;
				}
			}
		}
		else if(other.tag == "Spikes")
		{
			Spikes();
		}
		
		else if(other.tag == "WinZone")
		{
			Debug.Log ("adf");
			myPlayer.AddScore();
			HandleDeath();
		}
	}
	
	public void HitCollide()
	{
		if(attackHitBox.transform.localPosition.x > 0 && attackHitBox.transform.localPosition.x > Mathf.Abs(attackHitBox.transform.localPosition.y))
		{
			xMovement -= 5f;
			Velocity.x -= 5f;
		}
		else if(attackHitBox.transform.localPosition.x < 0 && Mathf.Abs(attackHitBox.transform.localPosition.x) > Mathf.Abs(attackHitBox.transform.localPosition.y))
		{
			xMovement += 5f;
			Velocity.x += 5f;
		}
		else if(attackHitBox.transform.localPosition.y > 0)
		{
			ySpeed -= 2f;
			Velocity.y -= 2f;
		}
		else
		{
			ySpeed += 2f;
			Velocity.y += 2f;
		}
	}
	
	public void HandleDeath()
	{
		deathParticleHolder = (GameObject)GameObject.Instantiate (DeathParticles) as GameObject;
		deathParticleHolder.transform.position = transform.position;
		
		myPlayer.PlayerDeath();
		
		if(!myPlayer.dead)
		{
			transform.position = SpawnPoint;
			Hit = false;
		}
	}
	
	public void Trap()
	{
		trapped = true;
		Hit = false;
		trappedTime = trapLength + Time.time;
		Velocity = Vector2.zero;
		ySpeed = 0;
		//rigidbody2D.velocity = Velocity;
	}
	
	public void Spikes()
	{
		HandleDeath ();
	}
}