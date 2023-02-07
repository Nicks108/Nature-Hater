using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NTTools.Editor 
{
    public class NTEditorWindow : EditorWindow
    {
        protected float Hight
        {
            get { return this.position.height; }
            set
            {
                this.position.Set(this.position.xMin, this.position.yMin, this.position.width, value);
            }
        }
        protected float Width
        {
            get { return this.position.width; }
            set
            {
                this.position.Set(this.position.xMin, this.position.yMin, value, this.position.height);
            }
        }
        protected float PercentageWidth(float input)
        {
            if (input >= 100)
                return this.Width;
            if (input <= 0)
                return 0;

            return Percentage(input, this.Width);
        }
        protected float PercentageHight(float input)
        {
            if (input >= 100)
                return this.Hight;
            if (input <= 0)
                return 0;

            return Percentage(input, this.Hight);
        }

        protected float Percentage(float percentage, float total)
        {
            percentage /= 100;
            return total * percentage;
        }
    }
}
