using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace POSSampleOWN.shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerTier
{
    None,
    Silver,
    Gold,
    Platinum
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RedemptionStatus
{
    Pending,
    Fulfilled,
    Cancelled
}