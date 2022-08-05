/* 
 * TinyTweener Class
 * (C)2022 Marco Capelli
 * 
 * I wrote this class because i needed something like iTween or DOTween
 * but without installing any external library for my project.
 * TinyTweener it's a quick and fast general utility to move objects
 * change values with very precise transitions. It also offer many 
 * options of use for different conditions.
 *
 * Features:
 * 
 * MoveTo()
 * RotateTo()
 * ColorTo()
 * LightColorTo()
 * LightIntensityTo()
 * AudioFadeTo()
 * 
 */

using System.Collections;
using UnityEngine;

namespace LS.TinyTweener
{

    public class TinyTweener : MonoBehaviour
    {
        #region Parameters

        public AnimationCurve _curve;
        private IEnumerator _moveTo;
        private IEnumerator _rotateTo;
        private IEnumerator _colorTo;
        private IEnumerator _lightColorTo;
        private IEnumerator _lightIntensityTo;
        private IEnumerator _audioFadeTo;
        
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

        #region ColorTo()

        /// <summary>
        /// Call this method to change a color over time from one to another.
        /// example: ColorTo(_color, _materialGameObject, _shaderColorParameterName, 10);
        /// </summary>

        public void ColorTo(Color _color, GameObject _materialGO, string _shaderColorParameterName, float duration)
        {
            Cancel(_colorTo);
            _colorTo = ColorToCoroutine(_color, _materialGO, _shaderColorParameterName, duration);
            StartCoroutine(_colorTo);
        }

        private IEnumerator ColorToCoroutine(Color _color, GameObject _materialGO, string _shaderColorParameterName, float duration)
        {
            float startTime = Time.time;

            Material _material = _materialGO.GetComponent<Renderer>().material;
            Color startColor = _material.GetColor(_shaderColorParameterName);

            while (Time.time - startTime < duration)
            {
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                _material.color = Color.Lerp(startColor, _color, t);

                yield return null;
            }

            _material.color = _color;
        }

        #endregion

        #region LightColorTo()

        /// <summary>
        /// Call this method to change the light color over time from one to another.
        /// example: LightColorTo(_color, _lightGameObject, 10);
        /// </summary>
        public void LightColorTo(Color _color, GameObject _lightGO, float duration)
        {
            Cancel(_lightColorTo);
            _lightColorTo = LightColorToCoroutine(_color, _lightGO, duration);
            StartCoroutine(_lightColorTo);
        }

        private IEnumerator LightColorToCoroutine (Color _color, GameObject _lightGO, float duration)
        {
            float startTime = Time.time;

            Light _light = _lightGO.GetComponent<Light>();
            Color startColor = _light.color;

            while (Time.time - startTime < duration)
            {
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                _light.color = Color.Lerp(startColor, _color, t);

                yield return null;
            }

            _light.color = _color;
        }

        #endregion

        #region LightIntensityTo()

        /// <summary>
        /// Call this method to change the light intensity over time from one value to another.
        /// example: LightIntensityTo(_intensityFloat, _lightGameObject, 10);
        /// </summary>
        public void LightIntensityTo(float _intensity, GameObject _lightGO, float duration)
        {
            Cancel(_lightIntensityTo);
            _lightIntensityTo = LightIntensityToCoroutine(_intensity, _lightGO, duration);
            StartCoroutine(_lightIntensityTo);
        }

        private IEnumerator LightIntensityToCoroutine(float _intensity, GameObject _lightGO, float duration)
        {
            float startTime = Time.time;

            Light _light = _lightGO.GetComponent<Light>();
            float startIntensity = _light.intensity;

            while (Time.time - startTime < duration)
            {
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                _light.intensity = Mathf.Lerp(startIntensity, _intensity, t);

                yield return null;
            }

            _light.intensity = _intensity;
        }

        #endregion

        #region AudioVolumeTo()

        /// <summary>
        /// Call this method to change the volume of an audio source over time from one value to another.
        /// example: AudioVolumeTo(_audioSourceGameObject, _desiredVolumeValue, 10);
        /// </summary>
        public void AudioFadeTo(GameObject _audioGO, float _volume, float duration)
        {
            Cancel(_audioFadeTo);
            _audioFadeTo = AudioFadeToCoroutine(_audioGO, _volume, duration);
            StartCoroutine(_audioFadeTo);
        }

        private IEnumerator AudioFadeToCoroutine(GameObject _audioGO, float _volume, float duration)
        {
            float startTime = Time.time;

            AudioSource _audioSource = _audioGO.GetComponent<AudioSource>();
            float startVolume = _audioSource.volume;

            while (Time.time - startTime < duration)
            {
                float t = _curve.Evaluate((Time.time - startTime) / duration);

                _audioSource.volume = Mathf.Lerp(startVolume, _volume, t);

                yield return null;
            }

            _audioSource.volume = _volume;
        }

        #endregion

        /* This makes sure we don't create multiple coroutines that fight for the current
         * transform.position and rotation. Only one instance of the coroutine runs. */
        public void Cancel(IEnumerator _coroutine)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
    }
}
