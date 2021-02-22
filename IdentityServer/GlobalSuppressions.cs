// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;


// Todo implement state validation on the authorization flow
[assembly:
    SuppressMessage("Style",
                    "IDE0060:Remove unused parameter",
                    Justification =
                        "This error message is suppressed because there was not enough time to implement the state checking, although not critical it would be better to have.",
                    Scope = "member",
                    Target =
                        "~M:IdentityServer.ExternalController.Callback(System.String,System.String,System.String)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly:
    SuppressMessage("Redundancy",
                    "RCS1163:Unused parameter.",
                    Justification =
                        "This error message is suppressed because there was not enough time to implement the state checking, although not critical it would be better to have.",
                    Scope = "member",
                    Target =
                        "~M:IdentityServer.ExternalController.Callback(System.String,System.String,System.String)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
