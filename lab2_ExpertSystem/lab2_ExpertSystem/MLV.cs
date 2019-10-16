using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_ExpertSystem
{
    class MLV
    {
        Rules[] rules = SerializerXML.rules; //список всех правил
        string ans = " ";
        //установить значения правил по умолчанию,
        //чтобы они не повторялись в рабочем списке правил
        public void SetRulesDefault()
        {
            for (int i = 0; i < rules.Length; i++)
            {
                rules[i].isExist = false;
                rules[i].isOld = false;
            }
        }
        public void UpdateRules(WorkListRules wlr, Work_memory wm, bool isBegin) //обноваление рабочего спика правил
        {
            for (int i = 0; i < rules.Length; i++)
            {
                if (!rules[i].isExist)
                {
                    if (checkCond(rules[i]))
                    {
                        RuleIsOld(rules[i]);
                    }

                    void RuleIsOld(Rules r)
                    {
                        if (r.isOld || isBegin) wlr.Add(r);
                        else wlr.Insert(0, r);

                        r.isExist = true;
                        r.isOld = true;
                    }
                }
            }

            bool checkCond(Rules r)
            {
                bool ruleIsValid = false;
                bool orCond = false;
                int k_or = 0;
                for (int j = 0; j < r.Conds.Length; j++)
                {
                    string cond = r.Conds[j];
                    bool ExistOr = false;
                    if (cond.Contains(@"or """))
                    {
                        ExistOr = true;
                        k_or++;
                    }

                    if (!orCond && !ExistOr && k_or > 0)
                        return false;

                    if (!(ExistOr && orCond))
                    {
                        cond = cond.Substring(cond.IndexOf(@"""") + 1);
                        cond = cond.Remove(cond.LastIndexOf(@""""));
                        string x = cond.Substring(0, cond.IndexOf(@""""));
                        string y = cond.Substring(cond.LastIndexOf(@"""") + 1);
                        if (y != "null")
                        {
                            bool pass = false;
                            for (int i = 0; i < wm.key.Count(); i++)
                            {
                                if (x == wm.key[i] && y == wm.value[i])
                                {
                                    if (ExistOr)
                                        orCond = true;
                                    pass = true;
                                    break;
                                }
                            }
                            if (pass) ruleIsValid = true;
                            else ruleIsValid = false;

                            if (!ruleIsValid && !ExistOr) break;
                        }
                        else ruleIsValid = true;
                    }
                }
                return ruleIsValid;
            }
        }

        string currentRule = null; //текущее правило
        public bool finish = false; //критерий завершения программы (если есть ответ)
        public bool isReturn = false; //критерий: либо выходить из цикла и дать поль-лю ответ, либо продолжить цикл с готовом ответом

        //метод выдачи либо:
        //1) вопроса поль-лю 
        //2) ответа поль-лю 
        //3) ответа программе для авто-обработки в цикле
        public string Question(WorkListRules wlr)
        {
            Random rnd = new Random();
            isReturn = false;
            finish = false;
            string str;
            try
            {
                str = wlr[0].Ans;
                currentRule = wlr[0].Name;
                wlr.RemoveAt(0);
            }
            catch
            {
                isReturn = true;
                finish = true;
                return "ответ: неизвестно из-за отсутствия подходящих правил!";
            }

            if (str.Contains("func"))
            {
                isReturn = true;
                str = str.Substring(str.IndexOf(@"(""") + 2);
                str = str.Remove(str.LastIndexOf(@""""));
                return str;
            }
            else
            {
                if (str.Contains(@"""да"""))
                    return "да";
                else if (str.Contains(@"""нет"""))
                    return "нет";

                if (str.Contains("ответ"))
                {
                    isReturn = true;
                    finish = true;
                    return "ответ: " + currentRule;
                }
            }
            return null;
        }

        public string SetAnswer(string ans, Work_memory wm) //установка факту текущего правила значение "да" или "нет"
        {
            if (ans.Contains("да")) ans = "да";
            else if (ans.Contains("нет")) ans = "нет";
            else ans = null;

            if (currentRule != null && ans != null)
            {
                wm.key.Add(currentRule);
                wm.value.Add(ans);
            }
            return ans;
        }

        public void SetRuless()
        {
            ans = " ";
        }
        public string SetRules(string s)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                if (s.Contains(rules[i].Name))
                {
                    for (int k = 0; k < rules[i].Conds.Length; k++)
                    {
                        if (PodSchut(rules[i].Conds[k]) && !ans.Contains(rules[i].Conds[k]) && !rules[i].Conds[k].Contains("null"))
                            ans += "\r\n> " + rules[i].Conds[k];
                        if (!rules[i].Conds[k].Contains("null"))
                        {
                            SetRules(rules[i].Conds[k]);
                        }
                    }
                }
            }
            return ans;
        }

        public bool PodSchut (string s)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                if (s.Contains(rules[i].Name))
                {
                    for (int k = 0; k < rules[i].Conds.Length; k++)
                    {
                        if (!rules[i].Conds[k].Contains("null") )
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}
