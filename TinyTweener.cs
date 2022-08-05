/* 
 * TinyTweener Class
 * (C)2022 Marco Capelli
 * 
 * I wrote this class because i needed something like iTween or DOTween
 * but without installing any external library for my project.
 * TinyTweener is quick and fast, especially if you need precise value
 * transitioning. It also offer many options of use for different conditions.
 * 
 */

using System.Collections;
using UnityEngine;

namespace LodaleSolution
{

    public class TinyTweener : MonoBehaviour
    {
        #region Parameters

        public AnimationCurve _curve;
        private IEnumerator _moveTo;
        private IEnumerator _rotateTo;
        
        #endregion

        #region MoveTo()
        /// <summary>
        /// Call this method to move a transform to a given target position and rotation.
        /// Pass the value of the Transform you want to control, the vectors and quaternions
        /// of the target position, and finally the duration time you need to perform the action.
        /// 
        /// example: MoveTo(this.transform, _target.transform.position, _target.transform.rotation, 10);
        /// </summary>
        public void MoveTo(Transform _transform, Vector3 _moveToPos, Quaternion _moveToRot, float duration)
        {
            Cancel(_moveTo);
            _moveTo = MoveToCoroutine(_transform, _moveToPos, _moveToRot, duration);
            StartCoroutine(_moveTo);
        }

        private IEnumerator MoveToCoroutine(Transform _transform, Vector3 _moveToPos, Quaternion _moveToRot, float duration)
        {
            // Store the start time and position of the given transform. Required for the linear interpolation.
            float startTime = Time.time;
            Vector3 startPos = _transform.position;
            Quaternion startRot = _transform.rotation;

            /* While the duration is not reached, move the position towards the snap position,
             * increasing the value in equal steps for the given duration, every frame. */
            while (Time.time - startTime < duration)
            {
                /* Gives us a value starting from 0 and ending in 1 as time progresses towards
                 * our duration, we will feed this 0-1 value into our linear interpolation. */
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                // This method performs better and faster than manually assigning transform.position and transform.rotation.
                _transform.SetPositionAndRotation(Vector3.Lerp(startPos, _moveToPos, t), Quaternion.Lerp(startRot, _moveToRot, t));

                yield return null;
            }

            /* And finally snap to the desired transform position to eliminate any tiny
             * floating point differences due to timing above */
            _transform.SetPositionAndRotation(_moveToPos, _moveToRot);
        }

        #endregion

        #region RotateTo()

        /// <summary>
        /// Call this method to rotate a transform to a given target rotation.
        /// Pass the value of the Transform you want to control and the quaternions
        /// of the target rotation, and finally the duration time you need to perform the action.
        /// 
        /// example: RotateTo(this.transform, _target.transform.position, _target.transform.rotation, 10);
        /// </summary>

        public void RotateTo(Transform _transform, Quaternion _rotateToRot, float duration)
        {
            Cancel(_rotateTo);
            _rotateTo = RotateToCoroutine(_transform, _rotateToRot, duration);
            StartCoroutine(_rotateTo);
        }

        private IEnumerator RotateToCoroutine(Transform _transform, Quaternion _rotateToRot, float duration)
        {
            float startTime = Time.time;
            Vector3 startPos = _transform.position;
            Quaternion startRot = _transform.rotation;

            while (Time.time - startTime < duration)
            {
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                _transform.SetPositionAndRotation(Vector3.Lerp(startPos, startPos, t), Quaternion.Lerp(startRot, _rotateToRot, t));

                yield return null;
            }

            _transform.SetPositionAndRotation(startPos, _rotateToRot);
        }

        #endregion

        public void Cancel(IEnumerator _coroutine)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
    }
}
