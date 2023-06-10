using System;
namespace KRR_Proj
{
	public static class Utils
	{
        //public static PrintList<T>(List<T>)

        public static IEnumerable<List<T>> IterateProductOfList<T>(List<T> values, int start_index, IEnumerable<T> candidate_vals)
        {
            Stack<(List<T> values, int index)> exec_stack = new Stack<(List<T> values, int index)>();

            exec_stack.Push((values, start_index));

            while (exec_stack.Count > 0)
            {
                var (now_vals, index) = exec_stack.Pop();
                if (index >= values.Count)
                {
                    yield return new List<T>(now_vals);
                }
                else
                {
                    foreach(var val in candidate_vals)
                    {
                        //Console.WriteLine($"Val:{val}");
                        var next_values = new List<T>(now_vals);
                        next_values[index] = val;
                        //Console.WriteLine("Next Values:");
                        //next_values.ForEach(x => Console.Write(x + ","));
                        //Console.WriteLine();


                        exec_stack.Push((new List<T>(next_values), index + 1));
                    }
                }
            }
        }
    }
}

