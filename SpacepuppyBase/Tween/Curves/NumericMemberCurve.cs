﻿using System;

using com.spacepuppy.Utils;

namespace com.spacepuppy.Tween.Curves
{

    [CustomMemberCurve(typeof(float), priority = int.MinValue)]
    [CustomMemberCurve(typeof(double), priority=int.MinValue)]
    [CustomMemberCurve(typeof(decimal), priority = int.MinValue)]
    [CustomMemberCurve(typeof(sbyte), priority = int.MinValue)]
    [CustomMemberCurve(typeof(int), priority = int.MinValue)]
    [CustomMemberCurve(typeof(long), priority = int.MinValue)]
    [CustomMemberCurve(typeof(byte), priority = int.MinValue)]
    [CustomMemberCurve(typeof(uint), priority = int.MinValue)]
    [CustomMemberCurve(typeof(ulong), priority = int.MinValue)]
    public class NumericMemberCurve : MemberCurve, ISupportRedirectToMemberCurve
    {

        #region Fields

        private double _start;
        private double _end;
        private TypeCode _numericType = TypeCode.Double;

        #endregion

        #region CONSTRUCTOR

        protected NumericMemberCurve()
        {

        }

        public NumericMemberCurve(string propName, float dur, double start, double end)
            : base(propName, dur)
        {
            _start = start;
            _end = end;
        }

        public NumericMemberCurve(string propName, Ease ease, float dur, double start, double end)
            : base(propName, ease, dur)
        {
            _start = start;
            _end = end;
        }

        protected override void ReflectiveInit(System.Type memberType, object start, object end, object option)
        {
            _start = ConvertUtil.ToDouble(start);
            _end = ConvertUtil.ToDouble(end);
            if (memberType != null && ConvertUtil.IsNumericType(memberType))
                _numericType = System.Type.GetTypeCode(memberType);
            else
                _numericType = TypeCode.Double;
        }

        void ISupportRedirectToMemberCurve.ConfigureAsRedirectTo(System.Type memberType, float totalDur, object current, object start, object end, object option)
        {
            var c = ConvertUtil.ToDouble(current);
            var s = ConvertUtil.ToDouble(_start);
            var e = ConvertUtil.ToDouble(end);
            _start = c;
            _end = e;

            c -= e;
            s -= e;
            this.Duration = (float)(System.Math.Abs(s) < MathUtil.DBL_EPSILON ? 0f : totalDur * c / s);

            if (memberType != null && ConvertUtil.IsNumericType(memberType))
                _numericType = System.Type.GetTypeCode(memberType);
            else
                _numericType = TypeCode.Double;
        }

        #endregion

        #region Properties

        public double Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public double End
        {
            get { return _end; }
            set { _end = value; }
        }

        internal TypeCode NumericType
        {
            get { return _numericType; }
            set
            {
                if (!ConvertUtil.IsNumericType(value)) throw new System.ArgumentException("TypeCode must be a numeric type.", "value");
                _numericType = value;
            }
        }

        #endregion

        #region MemberCurve Interface

        protected override object GetValueAt(float dt, float t)
        {
            if (this.Duration == 0) return ConvertUtil.ToPrim(_end, _numericType);
            return ConvertUtil.ToPrim(this.Ease(t, (float)_start, (float)_end - (float)_start, this.Duration), _numericType);
        }

        #endregion

    }
}
