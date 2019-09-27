using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_ExpertSystem
{
    class Controller
    {
        WorkListRules wlr;
        Work_memory wm;
        MLV mlv;
        public string path { get; set; }

        bool isStart = false;

        public string Input(string str)
        {
            if (!isStart)
            {
                if (str.Contains("(load "))
                {
                    //try
                    {
                        SerializerXML.SerializeXML(path);
                        wlr = new WorkListRules();
                        wm = new Work_memory();
                        mlv = new MLV();
                        return "Данные успешно подгружены!";
                    }
                    //catch { return "Ошибка в загрузке данных!"; }
                }
                else if (str.Contains("(reset)"))
                {
                    try
                    {
                        wlr.Clear();
                        wm.key.Clear();
                        wm.value.Clear();
                        isStart = false;
                        return "Данные очищены!";
                    }
                    catch { return "Нет данных, очищать нечего!"; }
                }
                else if (str.Contains("(run)"))
                {
                    //try
                    {
                        mlv.UpdateFactsAndRules(wlr, wm);
                        isStart = true;
                        return "Запуск алгоритма! Нажмите Enter чтобы начать.";
                    }
                    //catch { return "Нет данных, запуск алгоритма невозможен!"; }
                }
                else return null;
            }
            else
            {
                mlv.SetAnswer(str, wm);
                mlv.UpdateFactsAndRules(wlr, wm);
                return mlv.Question(wlr, wm);
            }
        }
    }
}
