using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_ExpertSystem
{
    class MLV
    {
        Rules[] rules = SerializerXML.rules;
        public void UpdateFactsAndRules(WorkListRules wlr, Work_memory wm)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                if (!rules[i].isExist)
                {
                    if (checkCond(rules[i]))
                    {
                        wlr.Add(rules[i]);
                        rules[i].isExist = true;
                    }
                }
            }

            bool checkCond(Rules r)
            {
                bool ruleIsValid = false;
                bool orCond = false;
                for (int j = 0; j < r.Conds.Length; j++)
                {
                    string cond = r.Conds[j];
                    bool ExistOr = false;
                    if (cond.Contains(@"or """)) ExistOr = true;

                    if (!(ExistOr && orCond))
                    {
                        //cond = cond.Substring(1, cond.Length - 1);

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

                            if (!ruleIsValid) break;
                        }
                        else ruleIsValid = true;
                    }
                }
                return ruleIsValid;
            }
        }

        string currentRule = null;
        public string Question(WorkListRules wlr, Work_memory wm)
        {
            string str = wlr[0].Ans;
            currentRule = wlr[0].Name;
            wlr.RemoveAt(0);
            if (str.Contains("func"))
            {
                str = str.Substring(str.IndexOf(@"(""") + 2);
                str = str.Remove(str.LastIndexOf(@""""));
                return str;
            }
            else
            {
                wm.key.Add(currentRule);
                if (str.Contains(@"""да"""))
                    wm.value.Add("да");
                else wm.value.Add("нет");
                return null;
            }
        }

        public string SetAnswer(string ans, Work_memory wm)
        {
            ans = ans.Substring(1);
            if ((ans == "да" || ans == "нет") && ans != null)
            {
                wm.key.Add(currentRule);
                wm.value.Add(ans);
            }
            return null;
        }
    }
}
