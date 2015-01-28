using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ThirdPersonController : MonoBehaviour
    {

        public AnimationClip idleAnimation;
        public AnimationClip walkAnimation;
        public AnimationClip runAnimation;
        public AnimationClip jumpPoseAnimation;
        public AnimationClip attackAnimation;
        public float walkMaxAnimationSpeed = 0.75f;
        public float trotMaxAnimationSpeed = 1.0f;
        public float runMaxAnimationSpeed = 1.0f;
        public float jumpAnimationSpeed = 1.15f;
        public float landAnimationSpeed = 1.0f;

        public string VerticalAxisName = "";
        public string HorizontalAxisName = "";
        public string JumpButton = "";
        public string AttackButton = "";

        private Animation _animation;

        private enum CharacterState
        {
            Idle = 0,
            Walking = 1,
            Trotting = 2,
            Running = 3,
            Jumping = 4,
            Attacking = 5,
        }

        private CharacterState _characterState;

        // The speed when walking
        public float walkSpeed = 2.0f;
        // after trotAfterSeconds of walking we trot with trotSpeed
        public float trotSpeed = 4.0f;
        // when pressing "Fire3" button (cmd) we start running
        public float runSpeed = 6.0f;

        public float inAirControlAcceleration = 3.0f;

        // How high do we jump when pressing jump and letting go immediately
        public float jumpHeight = 0.5f;

        // The gravity for the character
        public float gravity = 20.0f;
        // The gravity in controlled descent mode
        public float speedSmoothing = 10.0f;
        //var rotateSpeed = 500.0;
        public float trotAfterSeconds = 3.0f;

        private bool canJump = true;

        private float jumpRepeatTime = 0.05f;
        private float jumpTimeout = 0.15f;
        private float groundedTimeout = 0.25f;

        // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
        private float lockCameraTimer = 0.0f;

        // The current move direction in x-z
        private Vector3 moveDirection = Vector3.zero;
        // The current vertical speed
        private float verticalSpeed = 0.0f;
        // The current x-z move speed
        private float moveSpeed = 0.0f;

        // The last collision flags returned from controller.Move
        private CollisionFlags collisionFlags;

        // Are we jumping? (Initiated with jump button and not grounded yet)
        private bool jumping = false;
        private bool jumpingReachedApex = false;

        private bool attacking = false;

        // Are we moving backwards (This locks the camera to not do a 180 degree spin)
        private bool movingBack = false;
        // Is the user pressing any keys?
        private bool isMoving = false;
        // When did the user start walking (Used for going into trot after a while)
        private float walkTimeStart = 0.0f;
        // Last time the jump button was clicked down
        private float lastJumpButtonTime = -10.0f;
        // Last time we performed a jump
        private float lastJumpTime = -1.0f;


        // the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
        private float lastJumpStartHeight = 0.0f;


        private Vector3 inAirVelocity = Vector3.zero;

        private float lastGroundedTime = 0.0f;


        public bool isControllable = true;

        private void Awake()
        {
            moveDirection = transform.TransformDirection(Vector3.forward);
        }


        private void UpdateSmoothedMovementDirection()
        {
            var cameraTransform = Camera.main.transform;
            var grounded = IsGrounded();

            // Forward vector relative to the camera along the x-z plane	
            var forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            // Right vector relative to the camera
            // Always orthogonal to the forward vector
            var right = new Vector3(forward.z, 0, -forward.x);

            var v = Input.GetAxisRaw(VerticalAxisName);
            var h = Input.GetAxisRaw(HorizontalAxisName);

            // Are we moving backwards or looking backwards
            if (v < -0.2)
                movingBack = true;
            else
                movingBack = false;

            var wasMoving = isMoving;
            isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

            // Target direction relative to the camera
            var targetDirection = h * right + v * forward;

            // Grounded controls
            if (grounded)
            {
                // Lock camera for short period when transitioning moving & standing still
                lockCameraTimer += Time.deltaTime;
                if (isMoving != wasMoving)
                    lockCameraTimer = 0.0f;

                // We store speed and direction seperately,
                // so that when the character stands still we still have a valid forward direction
                // moveDirection is always normalized, and we only update it if there is user input.
                if (targetDirection != Vector3.zero)
                {
                    // If we are really slow, just snap to the target direction
                    //if (moveSpeed < walkSpeed * 0.9 && grounded)
                    //{
                    moveDirection = targetDirection.normalized;
                    //}
                    //    // Otherwise smoothly turn towards it
                    //else
                    //{
                    //    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);

                    //    moveDirection = moveDirection.normalized;
                    //}
                }

                // Smooth the speed based on the current target direction
                var curSmooth = speedSmoothing * Time.deltaTime;

                // Choose target speed
                //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
                var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

                _characterState = CharacterState.Idle;

                // Pick speed modifier
                if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
                {
                    targetSpeed *= runSpeed;
                    _characterState = CharacterState.Running;
                }
                else if (Time.time - trotAfterSeconds > walkTimeStart)
                {
                    targetSpeed *= trotSpeed;
                    _characterState = CharacterState.Trotting;
                }
                else
                {
                    targetSpeed *= walkSpeed;
                    _characterState = CharacterState.Walking;
                }

                moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

                // Reset walk time start when we slow down
                if (moveSpeed < walkSpeed * 0.3)
                    walkTimeStart = Time.time;
            }
            // In air controls
            else
            {
                // Lock camera while in air
                if (jumping)
                    lockCameraTimer = 0.0f;

                if (isMoving)
                    inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
            }



        }


        private void ApplyJumping()
        {
            // Prevent jumping too fast after each other
            if (lastJumpTime + jumpRepeatTime > Time.time)
                return;

            if (IsGrounded())
            {
                // Jump
                // - Only when pressing the button down
                // - With a timeout so you can press the button slightly before landing		
                if (canJump && Time.time < lastJumpButtonTime + jumpTimeout)
                {
                    verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
                    SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
                }
            }
        }


        private void ApplyGravity()
        {
            if (isControllable) // don't move player at all if not controllable.
            {
                // Apply gravity
                var jumpButton = Input.GetButton(JumpButton);


                // When we reach the apex of the jump we send out a message
                if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
                {
                    jumpingReachedApex = true;
                    SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
                }

                if (IsGrounded())
                    verticalSpeed = 0.0f;
                else
                    verticalSpeed -= gravity * Time.deltaTime;
            }
        }

        private float CalculateJumpVerticalSpeed(float targetJumpHeight)
        {
            // From the jump height and gravity we deduce the upwards speed 
            // for the character to reach at the apex.
            return Mathf.Sqrt(2 * targetJumpHeight * gravity);
        }

        private void DidJump()
        {
            jumping = true;
            jumpingReachedApex = false;
            lastJumpTime = Time.time;
            lastJumpStartHeight = transform.position.y;
            lastJumpButtonTime = -10;

            _characterState = CharacterState.Jumping;
        }

        private void DidAttack()
        {
            attacking = true;

            _characterState = CharacterState.Attacking;
        }

        private void Update()
        {
            if (Network.isClient)
                return;

            if (!isControllable)
            {
                // kill all inputs if not controllable.
                Input.ResetInputAxes();
            }
            if (Input.GetButtonDown(AttackButton))
            {
                DidAttack();
            }

            if (Input.GetButtonDown(JumpButton))
            {
                lastJumpButtonTime = Time.time;
            }

            UpdateSmoothedMovementDirection();

            // Apply gravity
            // - extra power jump modifies gravity
            // - controlledDescent mode modifies gravity
            ApplyGravity();

            // Apply jumping logic
            ApplyJumping();

            // Calculate actual motion
            var movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
            movement *= Time.deltaTime;

            // Move the controller
            var controller = GetComponent<CharacterController>();
            collisionFlags = controller.Move(movement);

            // Set rotation to the move direction
            if (IsGrounded())
            {

                transform.rotation = Quaternion.LookRotation(moveDirection);

            }
            else
            {
                var xzMove = movement;
                xzMove.y = 0;
                if (xzMove.sqrMagnitude > 0.001)
                {
                    transform.rotation = Quaternion.LookRotation(xzMove);
                }
            }

            // We are in jump mode but just became grounded
            if (IsGrounded())
            {
                lastGroundedTime = Time.time;
                inAirVelocity = Vector3.zero;
                if (jumping)
                {
                    jumping = false;
                    SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //	Debug.DrawRay(hit.point, hit.normal);
            if (hit.moveDirection.y > 0.01)
                return;
        }

        private float GetSpeed()
        {
            return moveSpeed;
        }

        private bool IsJumping()
        {
            return jumping;
        }

        private bool IsAttacking()
        {
            return attacking;
        }

        private bool IsGrounded()
        {
            return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
        }

        private Vector3 GetDirection()
        {
            return moveDirection;
        }

        private bool IsMovingBackwards()
        {
            return movingBack;
        }

        private float GetLockCameraTimer()
        {
            return lockCameraTimer;
        }

        private bool IsMoving()
        {
            return Mathf.Abs(Input.GetAxisRaw(VerticalAxisName)) + Mathf.Abs(Input.GetAxisRaw(HorizontalAxisName)) > 0.5;
        }

        private bool HasJumpReachedApex()
        {
            return jumpingReachedApex;
        }

        private bool IsGroundedWithTimeout()
        {
            return lastGroundedTime + groundedTimeout > Time.time;
        }

        private void Reset()
        {
            gameObject.tag = "Player";
        }
    }
}