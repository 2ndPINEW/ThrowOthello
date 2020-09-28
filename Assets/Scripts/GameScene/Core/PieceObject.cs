using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThrowOthello
{
    public class PieceObject : MonoBehaviour
    {

        public int PileIndex = 0;

        bool systemOperating = false;
        bool positionSmoothMoving = false;
        bool rotationSmoothMoving = false;

        Vector3 moveTargetPosition = Vector3.zero;
        Vector3 VelocityPosition = Vector3.zero;

        Vector3 moveTargetRotation = Vector3.zero;
        Vector3 velocityRotation = Vector3.zero;

        Rigidbody rigidbody;


        public Color PieceColor()
        {
            if (-AppSetting.Epsilon < this.transform.forward.y && this.transform.forward.y < AppSetting.Epsilon)
                return Color.gray;
            if (this.transform.forward.y > 0)
                return Color.white;
            if(this.transform.forward.y < 0)
                return Color.black;
            return Color.gray;
        }

        public Color RPieceColor()
        {
            if (-AppSetting.Epsilon < this.transform.forward.y && this.transform.forward.y < AppSetting.Epsilon)
                return Color.gray;
            if (this.transform.forward.y > 0)
                return Color.black;
            if (this.transform.forward.y < 0)
                return Color.white;
            return Color.gray;
        }


        public bool isRedy()
        {
            if (systemOperating) return false;
            if (positionSmoothMoving) return false;
            if (rotationSmoothMoving) return false;
            if (!rigidbody.IsSleeping()) return false;

            return true;
        }


        public float GetHeight()
        {
            return transform.position.y;
        }


        public Color GetScoreBaseColor()
        {
            //駒がフィールドの外にいる場合
            if (!transform.position.ToPosition().ToPositionIndex().isWithinRange()) return Color.gray;

            return PieceColor();
        }


        private void Awake()
        {
            rigidbody = this.gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (systemOperating && positionSmoothMoving)
            {
                this.transform.position = Vector3.SmoothDamp(transform.position, moveTargetPosition, ref VelocityPosition, AppSetting.PieceMoveSmoothTime);
            }

            if (systemOperating && rotationSmoothMoving)
            {
                var v = Vector3.SmoothDamp(transform.eulerAngles, moveTargetRotation, ref velocityRotation, AppSetting.PieceRotateSmoothTime);
                this.transform.rotation = Quaternion.Euler(v);
            }
        }


        public void StartOrganize(PositionIndex positionIndex)
        {
            if (systemOperating) return;
            if (!positionIndex.isWithinRange()) return;
            StartCoroutine(Organize(positionIndex));
        }


        public void StartReverse(PositionIndex positionIndex, Color targetColor)
        {
            if (systemOperating) return;
            if (!positionIndex.isWithinRange()) return;
            StartCoroutine(Reverse(positionIndex, targetColor));
        }


        IEnumerator Reverse(PositionIndex positionIndex, Color targetColor)
        {
            if (PieceColor() == Color.gray) yield break;
            if (targetColor == PieceColor()) yield break;
            systemOperating = true;

            switchPhysics(false);
            positionSmoothMoving = true;
            moveTargetPosition = positionIndex.ToPosition().ToVector3((PileIndex + 1) * FieldSetting.PieceMoveHeight);
            yield return new WaitForSeconds(0.05f);

            float X = 0;
            if (PieceColor() == Color.black)
            {
                transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                X = 90;
            }
            if (PieceColor() == Color.white)
            {
                transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
                X = 270;
            }

            for(int count = 0; count < 180; count+=AppSetting.PieceReverseSpeed)
            {
                transform.rotation = Quaternion.Euler(new Vector3(X+=AppSetting.PieceReverseSpeed, 0, 0));
                yield return null;
            }

            positionSmoothMoving = false;
            switchPhysics(true);
            yield return new WaitForSeconds(0.5f);
            systemOperating = false;
        }


        IEnumerator Organize(PositionIndex positionIndex)
        {
            systemOperating = true;
            switchPhysics(false);
            StartCoroutine(positionOrganize(positionIndex));
            StartCoroutine(rotationOrganize(PieceColor()));
            yield return new WaitForSeconds(1f);
            switchPhysics(true);
            yield return new WaitForSeconds(0.5f);
            systemOperating = false;
        }


        IEnumerator rotationOrganize(Color color)
        {
            if (color == Color.gray)
                yield break;
            rotationSmoothMoving = true;
            transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x % 360, transform.eulerAngles.y % 360, transform.eulerAngles.z % 360));
            if(color == Color.black)
            {
                moveTargetRotation = new Vector3(90, 0, 0);
            }
            if (color == Color.white)
            {
                moveTargetRotation = new Vector3(270, 0, 0);
            }
            yield return new WaitForSeconds(1);
            rotationSmoothMoving = false;
        }


        IEnumerator positionOrganize(PositionIndex positionIndex)
        {
            positionSmoothMoving = true;
            moveTargetPosition = positionIndex.ToPosition().ToVector3((PileIndex+1)*FieldSetting.PieceMoveHeight);
            yield return new WaitForSeconds(1);
            positionSmoothMoving = false;
        }


        void switchPhysics(bool value)
        {
            //gameObject.GetComponent<MeshCollider>().convex = value;
            if (value)
            {
                rigidbody.isKinematic = !value;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
            else
            {
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rigidbody.isKinematic = !value;
            }

        }
    }
}