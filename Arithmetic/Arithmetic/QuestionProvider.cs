using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class QuestionProvider
    {
        // description
        // 1. this class only have static function
        // 2. can provide questions and answers in string List form.
        // 3. can clear generate file.

        
        // CORE CODE (Tested): 从文件读取指定个题目, 如果文件大小和指定个数不同则产生异常. 如果指定个数为 -1, 则读取所有题目
        public static Tuple<List<string>, List<string>> read_QA_pairs_from_files(
            int amount, string question_path, string answer_path // 问题文件和答案文件的路径
            )
        {
            List<string> Q = new List<string>();
            List<string> A = new List<string>();

            // checking.
            if(File.Exists(question_path)== false || File.Exists(answer_path) == false)
            {
                throw new FileNotFoundException("this file does not exist!!");
            }
            int linecnt = Get_file_linecnt(question_path);
            if (linecnt != amount)
            {
                throw new Exception($"linecnt={linecnt}, but amount = {amount}. Question cnt not match. Do you forget to call Generator before this?");
            }

            // read file
            else
            {
                using (StreamReader sr_q = new StreamReader(question_path))
                {
                    using (StreamReader sr_a = new StreamReader(answer_path))
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            Q.Add(sr_q.ReadLine());
                            A.Add(sr_a.ReadLine());
                        }
                    }
                }
            }

            return new Tuple<List<string>, List<string>>(Q, A);
        }

        // facility
        private static int Get_file_linecnt(string filename)
        {
            int lineCount = 0;
            using (var reader = File.OpenText(filename))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }

        //// CORE CODE: 将给定的题目-答案对儿保存到文件 (APPEND MODE) 
        public void Save_to_file(
            string question_path, 
            string answer_path, 
            List<string> Ques,
            List<string> Ans)     
        {
            // checking
            if (Ques == null || Ans == null)
            {
                throw new NullReferenceException("QA list is null!");
            }
            if (Ques.Count == 0 || Ans.Count == 0)
            {
                throw new Exception("Empty QA list!");
            }

            // append to files
            using (FileStream fs = File.Open(question_path, FileMode.Append))
            {
                foreach (string expression_in_number in Ques)
                {
                    AddText(fs, expression_in_number + "\n");
                }
            }
            using (FileStream fs = File.Open(answer_path, FileMode.Append))
            {
                foreach (string ans in Ans)
                {
                    AddText(fs, ans + "\n");
                }
            }
        }
        
        // facility: write string to file
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
