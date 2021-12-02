using Europa.Commons;
using Europa.Extensions;
using NHibernate;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Shared.Log
{
    public sealed class ExceptionLogger
    {
        private ExceptionLogger()
        {
        }

        public static long LogException(Exception exc, string contextInfo = "", [CallerMemberName] string caller = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            long innerErrorId = 0;

            IStatelessSession session = null;
            try
            {
                session = NHibernateSession.StatelessSession();

                Exception toLog = exc;
                while (toLog != null)
                {
                    var error = new Error();
                    error.Type = toLog.GetType().ToString();
                    error.Message = toLog.Message;
                    error.Stacktrace = toLog.StackTrace;
                    error.Source = toLog.Source;
                    error.CodeLine = GetCodeLine(toLog, line);
                    error.OriginClass = GetTarget(toLog, path);
                    error.Caller = GetMethod(toLog, caller);
                    error.ContextInfo = contextInfo;
                    if (error.Message != null && error.Message.Length > 2000)
                    {
                        error.Message = error.Message.Substring(0, 1990) + "-truncate";
                    }
                    session.Insert(error);
                    innerErrorId = error.Id;
                    toLog = toLog.InnerException;
                }
            }
            catch (Exception inner)
            {
                ExceptionUtility.LogException(exc);
                ExceptionUtility.LogException(inner);
            }
            finally
            {
                session.CloseIfOpen();
            }
            return innerErrorId;
        }

        private static string GetClassName(string path)
        {
            return path.IsEmpty() ? null : Path.GetFileName(path);
        }

        private static long GetCodeLine(Exception exc, long line)
        {
            return new StackTrace(exc, true)?.GetFrame(0)?.GetFileLineNumber() ?? line;
        }

        private static string GetTarget(Exception exc, string path)
        {
            return exc.TargetSite != null && exc.TargetSite.ReflectedType != null ?
                exc.TargetSite.ReflectedType.Name
                : GetClassName(path);
        }

        private static string GetMethod(Exception exc, string caller)
        {
            return exc.TargetSite != null ?
                exc.TargetSite.Name
                : caller;
        }
    }
}
