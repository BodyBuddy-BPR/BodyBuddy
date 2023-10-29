using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Enums
{
    public enum Gender
    {
        Female = 0,
        Male = 1,
        None = 2
    }

    public enum UserActivity
    {
        VeryActive = 0,
        Active = 1,
        LittleActive = 2,
        NotVeryActive = 3,
    }

    public enum Goal
    {
        GainMuscle = 0,
        LoseWeight = 1,
    }

    public enum FocusArea
    {
        UpperBody = 0,
        LowerBody = 1,
        AbsAndCore = 2,
        Other = 3,
    }
}
