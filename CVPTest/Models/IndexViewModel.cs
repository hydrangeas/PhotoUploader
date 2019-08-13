using System.Collections.Generic;

namespace CVPTest.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<Job> jobs)
        {
            Jobs = jobs;
        }

        public IList<Job> Jobs { get; private set; }
    }
}
