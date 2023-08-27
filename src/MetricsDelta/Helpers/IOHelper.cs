using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsDelta.Helpers
{
    public static class IOHelper
    {
        #region Private Enums

        private enum WriteStatus
        {
            Started,
            Finishing,
            Finished
        }

        #endregion

        #region Public Methods

        public static TResult RepeatTillResultOrThrow<TResult>(Func<TResult> func, int numberOfTries, int waitTimeMS, int tryCounter)
        {
            try
            {
                return func.Invoke();
            }
            catch
            {
                if (tryCounter >= numberOfTries)
                    throw;

                Thread.Sleep(waitTimeMS);

                tryCounter++;
                return RepeatTillResultOrThrow(func, numberOfTries, waitTimeMS, tryCounter);
            }
        }

        #endregion
    }
}
