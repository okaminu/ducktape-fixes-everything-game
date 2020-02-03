using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Invector.vCharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {
        public Text infoText;
        public int yellowTapeCount;
        public int blueTapeCount;
        public int greenTapeCount;
        public Text yellowTapeCountText;
        public Text blueTapeCountText;
        public Text greenTapeCountText;
        private HashSet<string> yellowTapeTags = new HashSet<string>();
        private HashSet<string> blueTapeTags = new HashSet<string>();
        private HashSet<string> greenTapeTags = new HashSet<string>();
        public GameObject dogInteraction;
        public Animator duckAnimator;
        public GameObject fixedDog;
        public GameObject catInteraction;
        public GameObject fixedCat;
        public GameObject birdInteraction;
        public GameObject fixedBird;
        public GameObject fish;
        public GameObject[] oldPond;
        public GameObject box8;


        void Start() {
            box8.SetActive(false);
            dogInteraction.SetActive(true);
            fixedDog.SetActive(false);
            catInteraction.SetActive(true);
            fixedCat.SetActive(false);
            birdInteraction.SetActive(true);
            fixedBird.SetActive(false);
            infoText.enabled = false;
            yellowTapeCount = 0;
            blueTapeCount = 0;
            greenTapeCount = 0;
            yellowTapeTags.Add("yellowTape1");
            yellowTapeTags.Add("yellowTape2");
            blueTapeTags.Add("blueTape1");
            greenTapeTags.Add("greenTape1");

        }

        void Update()
        {
            float distDog = Vector3.Distance(dogInteraction.transform.position, transform.position);



            if (Input.GetKeyDown("space")) {
                if (distDog <= 2 && fixedDog.active == false && greenTapeCount > 0) {
                    fixedDog.SetActive(true);
                    dogInteraction.SetActive(false);
                    greenTapeCount--;
                    greenTapeCountText.text = greenTapeCount.ToString();
                }
            }

            float distCat = Vector3.Distance(catInteraction.transform.position, transform.position);



            if (Input.GetKeyDown("space"))
            {
                if (distCat <= 2 && fixedCat.active == false && yellowTapeCount > 0)
                {
                    fixedCat.SetActive(true);
                    catInteraction.SetActive(false);
                    yellowTapeCount--;
                    yellowTapeCountText.text = yellowTapeCount.ToString();
                }
            }

            float distBird = Vector3.Distance(birdInteraction.transform.position, transform.position);



            if (Input.GetKeyDown("space"))
            {
                if (distBird <= 2 && fixedBird.active == false && yellowTapeCount > 0)
                {
                    fixedBird.SetActive(true);
                    birdInteraction.SetActive(false);
                    yellowTapeCount--;
                    yellowTapeCountText.text = yellowTapeCount.ToString();
                }
            }



            float distFish = Vector3.Distance(fish.transform.position, transform.position);



            if (Input.GetKeyDown("space"))
            {
                if (distFish <= 3 && fish.active == true && blueTapeCount > 0)
                {
                    oldPond = GameObject.FindGameObjectsWithTag("OldPond");
                    foreach (GameObject pondObject in oldPond)
                    {
                        pondObject.SetActive(false);
                    }
                    box8.SetActive(true);
                    blueTapeCount--;
                    blueTapeCountText.text = blueTapeCount.ToString();
                }
            }

            if ((distDog <= 2 && dogInteraction.active == true) || (distCat <= 2 && catInteraction.active == true) ||
     (distBird <= 2 && birdInteraction.active == true) || (distFish <= 3 && fish.active == true))
            {
                infoText.enabled = true;
            }
            else
            {
                infoText.enabled = false;
            }

            if (isWalkKeyDown() && isRunKeyUp()) {
                    duckAnimator.SetInteger("speed", 1);
            }

            if (isWalkKeyUp() && isRunKeyUp())
            {
                duckAnimator.SetInteger("speed", 0);
            }

            if (isRunKeyDown() && isWalkKeyDown())
            {
                duckAnimator.SetInteger("speed", 2);
            }

            if (isRunKeyUp() && isWalkKeyDown())
            {
                duckAnimator.SetInteger("speed", 1);
            }
        }

        bool isRunKeyUp()
        {
            return !Input.GetKey(KeyCode.LeftShift);
        }

        bool isRunKeyDown()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        bool isWalkKeyDown()
        {
            return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        }

        bool isWalkKeyUp()
        {
            return (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D));
        }


        void OnTriggerEnter(Collider other)
        {
            if (yellowTapeTags.Contains(other.tag))
            {
                yellowTapeTags.Remove(other.tag);
                yellowTapeCount++;
                yellowTapeCountText.text = yellowTapeCount.ToString();
            }

            if (blueTapeTags.Contains(other.tag))
            {
                blueTapeTags.Remove(other.tag);
                blueTapeCount++;
                blueTapeCountText.text = blueTapeCount.ToString();
            }

            if (greenTapeTags.Contains(other.tag))
            {
                greenTapeTags.Remove(other.tag);
                greenTapeCount++;
                greenTapeCountText.text = greenTapeCount.ToString();
            }
        }

        public virtual void ControlAnimatorRootMotion()
        {
            if (!this.enabled) return;

            if (inputSmooth == Vector3.zero)
            {
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlLocomotionType()
        {
            if (lockMovement) return;

            if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
            {
                isStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlRotationType()
        {
            if (lockRotation) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                // calculate input smooth
                inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                RotateToDirection(dir);
            }
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (input.magnitude <= 0.01)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        public virtual void Sprint(bool value)
        {
            var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    isSprinting = false;
                }
            }
            else if (isSprinting)
            {
                isSprinting = false;
            }
        }

        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }
    }
}