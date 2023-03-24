using System.Collections;
using System.Collections.Generic;
using SpaceShooter.UserControl;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Масса.
        /// </summary>
        [Header("Space Ship")]
        [SerializeField] private float _mass;

        /// <summary>
        /// Полное ускорение прямолинейного движения.
        /// </summary>
        [SerializeField] private float _acceleration;

        /// <summary>
        /// Полное ускорение поворота.
        /// </summary>
        [SerializeField] private float _angularAcceleration;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float _maxVelocity;

        /// <summary>
        /// Максимальная скорость поворота, градус/с.
        /// </summary>
        [SerializeField] private float _maxAngularVelocity;

        /// <summary>
        /// Ось управления линейным ускорением (диапазон -1..1).
        /// </summary>
        public ControlAxis AccelerationAxis { get; private set; }

        /// <summary>
        /// Ось управления ускорением поворота (диапазон -1..1).
        /// </summary>
        public ControlAxis AngularAccelerationAxis { get; private set; }

        /// <summary>
        /// Ссылка на физическое тело корабля.
        /// </summary>
        private Rigidbody2D _myRigidbody;


        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        protected override void Start()
        {
            _myRigidbody = GetComponent<Rigidbody2D>();
            _myRigidbody.mass = _mass;
            _myRigidbody.inertia = 1;

            ControlAxis[] results = transform.parent.GetComponentsInChildren<ControlAxis>();
            foreach (ControlAxis axis in results)
            {
                if (axis.Nickname == "Acceleration Axis") AccelerationAxis = axis;
                else if (axis.Nickname == "Angular Acceleration Axis") AngularAccelerationAxis = axis;
            }
        }

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            ////осями больше здесь не управляем
            ////управление вперёд-назад
            //AccelerationAxis = 0f;
            //if (Input.GetKey(KeyCode.UpArrow)) AccelerationAxis = 1f;
            //else if (Input.GetKey(KeyCode.DownArrow)) AccelerationAxis = -1f;

            ////упраление вправо-влеов
            //AngularAccelerationAxis = 0f;
            //if (Input.GetKey(KeyCode.LeftArrow)) AngularAccelerationAxis = 1f;
            //else if (Input.GetKey(KeyCode.RightArrow)) AngularAccelerationAxis = -1f;
        }

        /// <summary>
        /// FixedUpdate запускается с фиксированным перидом.
        /// </summary>
        private void FixedUpdate()
        {
            //обработка физики движения корабля
            RigidBodyUpdate();
        }

        /// <summary>
        /// ОБработка физики движения корабля.
        /// </summary>
        private void RigidBodyUpdate()
        {
            //линейное движение
            //примененине команды от игрока
            _myRigidbody.AddForce(_acceleration * AccelerationAxis * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            //торможение шершавым космическим вакуумом
            _myRigidbody.AddForce(-_myRigidbody.velocity * (_acceleration / _maxVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            //поворот
            //примененине команды от игрока
            _myRigidbody.AddTorque(_angularAcceleration * AngularAccelerationAxis * Time.fixedDeltaTime, ForceMode2D.Force);
            //торможение шершавым космическим вакуумом
            _myRigidbody.AddTorque(-_myRigidbody.angularVelocity * (_angularAcceleration / _maxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}
