using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_ExpertSystem
{
    class Controller
    {
        WorkListRules wlr; //Рабочий список правил
        Work_memory wm; //Рабочая память
        MLV mlv; //Машина лог. вывода
        bool isBegin = true;
        public string path { get; set; } //путь файла

        bool isStarted = false; //критерий: запущена ли программа

        public string Input(string str, bool flag)
        {
            if (!isStarted)
            {
                if (str.Contains("(load ")) //обработка команды load
                {
                    try
                    {
                        SerializerXML.SerializeXML(path);
                        wlr = new WorkListRules();
                        wm = new Work_memory();
                        mlv = new MLV();
                        return "Данные успешно подгружены!";
                    }
                    catch { return "Ошибка в загрузке данных!"; }
                }
                else if (str.Contains("(reset)")) //обработка команды reset
                {
                    try
                    {
                        mlv.SetRulesDefault();
                        wlr.Clear();
                        wm.key.Clear();
                        wm.value.Clear();
                        mlv.SetRuless();
                        isStarted = false;
                        return "Данные очищены!";
                    }
                    catch { return "Нет данных, очищать нечего!"; }
                }
                else if (str.Contains("(run)")) //обработка команды run
                {
                    try
                    {
                        isStarted = true;
                        string print = null;
                        if (!flag)
                        {
                            mlv.finish = false;                          
                            print = "Запуск алгоритма!\r\n>\r\n>" + LoopMLV("", true);
                        }
                        else
                            print = "Запуск обратного алгоритма, введите гипотезу!\r\n>\r\n>" + Obr_LoopMLV("");
                        return print;
                    }
                    catch { return "Нет данных, запуск алгоритма невозможен!"; }
                }
                else return null;
            }
            else
            {
                if (!flag)
                {
                    if (str.Contains("да") || str.Contains("нет"))
                        return LoopMLV(str, false);
                }
                else
                {
                    isStarted = false;
                    return Obr_LoopMLV(str);
                }
            }
            return null;

            //Цикл млв для выдачи ответа пользователю:
            //1) либо выводит готовый ответ
            //2) либо выдаёт вопрос на который нужно ответить
            string LoopMLV(string s, bool isBegin) 
            {
                while (true)
                {
                    mlv.SetAnswer(s, wm);
                    mlv.UpdateRules(wlr, wm, isBegin);
                    isBegin = false;
                    string q = mlv.Question(wlr);
                    if (mlv.finish) isStarted = false;
                    if (mlv.isReturn)
                        return q;
                    else s = q;
                }
            }

            string Obr_LoopMLV(string s)
            {
                string output = null;
                if (s.Contains(">")) s = s.Remove(0, 1);
                    output += mlv.SetRules(s);
                
                    return output;
            }

        }
        public string Print_coment (bool fl)
        {
            string l = null;
            for (int i = 0; i < wm.value.Count; i++)
            {
                l += wm.key[i] + " = " + wm.value[i] +"\r\n>";
            }
            return l;
        }
    }
}
