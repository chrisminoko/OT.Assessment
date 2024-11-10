﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Core.ResponseMessages
{
    public static class Responses
    {
        public const string FailedToPublish = "Failed to publish, try again Later";
        public const string FailedToPublishProvider = "Failed to publish provider creation message";
        public const string FailedToPublishCasinoWager = "Failed to publish casino wager";
        public const string GeneralError = "Something went wrong, please try again !";
        public const string ErrorCreatingProvider = "Error creating provider"; 
        public const string ErrorCreatingPlayer = "Error creating player";
        public const string GeneralSuccess = "Operation successful";
    }
}
