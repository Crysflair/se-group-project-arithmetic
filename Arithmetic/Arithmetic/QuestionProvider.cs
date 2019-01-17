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

        //// CORE CODE: 从文件读取指定个题目, 如果文件大小和指定个数不同则产生异常. 如果指定个数为 -1, 则读取所有题目
        public static Tuple<List<string>, List<string>> read_QA_pairs_from_files(
            int amount, string question_path, string answer_path // 问题文件和答案文件的路径
            )
        {
            List<Tuple<string, string>> QA_pairs = new List<Tuple<string, string>>();

            // checking.
            int linecnt = Get_file_linecnt("./questions.txt");
            if (linecnt - 1 != amount)
            {
                throw new Exception("Question cnt not match. Do you forget to call Generator before this?");
            }
            // read file
            else
            {
                using (StreamReader sr_q = new StreamReader("./questions.txt"))
                {
                    using (StreamReader sr_a = new StreamReader("./questions.txt"))
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            QA_pairs.Add(new Tuple<string, string>(
                                sr_q.ReadLine(),
                                sr_a.ReadLine()
                                ));  
                        }
                    }
                }
            }

            return QA_pairs;
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
     //   3666^^^
        public void Save_to_file(
            string question_path, 
            string answer_path, 
            List<string> Ques,
            List<string> Ans)     
        {
            if (Ans.Count == 0)
            {
                throw new Exception("Empty expression list! Please call Generate before Save_to_file!");
            }

            using (FileStream fs = File.Open(question_path, FileMode.Create))
            {
                fs.Seek(0, SeekOrigin.End);
                foreach (string expression_in_number in Ques)
                {
                    AddText(fs, expression_in_number + "\n");
                }
            }
            using (FileStream fs = File.Open(answer_path, FileMode.Create))
            {
                fs.Seek(0, SeekOrigin.End);
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
