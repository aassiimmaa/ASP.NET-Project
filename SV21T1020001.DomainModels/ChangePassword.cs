﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020001.DomainModels
{
    public class ChangePassword
    {
        public string OldPassword { get; set; } = "";

        public string NewPassword { get; set; } = "";

        public string ConfirmPassword { get; set; } = "";

        public string SuccessMessage { get; set; } = "";
    }
}
