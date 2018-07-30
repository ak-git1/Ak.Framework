using System;
using System.Collections.Generic;
using System.Globalization;
using Ak.Framework.Core.Enums;
using Ak.Framework.Morphology.Enums;

namespace Ak.Framework.Morphology.Decliners
{
    /// <summary>
    /// Класс для склонения ФИО
    /// </summary>
    public class FioDecliner
    {
        #region Переменные и константы

        /// <summary>
        /// Словарь исключений склонений фамилий
        /// </summary>
        private static Dictionary<string, WordForms> _lastNamesCache;

        /// <summary>
        /// Словарь исключений склонений имен
        /// </summary>
        private static Dictionary<string, WordForms> _firstNamesCache;

        /// <summary>
        /// Словарь исключений склонений отчеств
        /// </summary>
        private static Dictionary<string, WordForms> _middleNamesCache;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор
        /// </summary>
        public FioDecliner()
        {
            
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="lastNamesForms">Строка со склонениями фамилий (В строке падежи расположены через пробел, новые фамилии через перенос строки)</param>
        /// <param name="firstNamesForms">Строка со склонениями имен (В строке падежи расположены через пробел, новые имена через перенос строки)</param>
        /// <param name="middleNamesForms">Строка со склонениями отчеств (В строке падежи расположены через пробел, новые отчества через перенос строки)</param>
        public FioDecliner(string lastNamesForms, string firstNamesForms, string middleNamesForms)
        {
            _lastNamesCache = WordForms.CreateWordFormsCache(lastNamesForms);
            _firstNamesCache = WordForms.CreateWordFormsCache(firstNamesForms);
            _middleNamesCache = WordForms.CreateWordFormsCache(middleNamesForms);
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Склонение ФИО
        /// </summary>
        /// <param name="fio">ФИО</param>
        /// <param name="wordsForm">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten">Сокращенная форма</param>
        /// <returns></returns>
        public static string DeclineFio(string fio, WordsForms wordsForm = WordsForms.Nominative, Genders gender = Genders.Unknown, bool shorten = false)
        {
            return Decline(fio, (int)wordsForm, (int)gender, shorten);
        }

        /// <summary>
        /// Склонение фамилии
        /// </summary>
        /// <param name="familyName">Фамилия</param>
        /// <param name="wordsForm">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten">Сокращенная форма</param>
        /// <returns></returns>
        public static string DeclineFamilyName(string familyName, WordsForms wordsForm = WordsForms.Nominative, Genders gender = Genders.Unknown, bool shorten = false)
        {
            return Decline(familyName, string.Empty, string.Empty, (int)wordsForm, (int)gender, shorten)[0];
        }

        /// <summary>
        /// Склонение имени
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="wordsForm">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten">Сокращенная форма</param>
        /// <returns></returns>
        public static string DeclineName(string name, WordsForms wordsForm = WordsForms.Nominative, Genders gender = Genders.Unknown, bool shorten = false)
        {
            return Decline(string.Empty, name, string.Empty, (int)wordsForm, (int)gender, shorten)[1];
        }

        /// <summary>
        /// Склонение отчества
        /// </summary>
        /// <param name="middleName">Фамилия</param>
        /// <param name="wordsForm">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten">Сокращенная форма</param>
        /// <returns></returns>
        public static string DeclineMiddleName(string middleName, WordsForms wordsForm = WordsForms.Nominative, Genders gender = Genders.Unknown, bool shorten = false)
        {
            return Decline(string.Empty, string.Empty, middleName, (int)wordsForm, (int)gender, shorten)[2];
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Склонение ФИО
        /// </summary>
        /// <param name="declinedSurname">Фамилия</param>
        /// <param name="declinedName">Имя</param>
        /// <param name="declinedMiddleName">Отчество</param>
        /// <param name="caseNumber">Падеж</param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten">Сокращенно</param>
        /// <returns>Возвращает массив из трех элементов [Фамилия, Имя, Отчество]</returns>
        private static string[] Decline(string declinedSurname, string declinedName, string declinedMiddleName, int caseNumber = 1, int gender = 0, bool shorten = false)
        {
            string surname = null;
            string name = null;
            string patronymic = null;
            string patronymicAfter = null;
            string patronymicBefore = null;
            bool isFeminine = false;
            int index = 0;
            string surnameNew = null;
            string surnameOld = null;

            surname = ProperCase(declinedSurname);
            name = ProperCase(declinedName);
            patronymic = ProperCase(declinedMiddleName);
            patronymicBefore = string.Empty;
            patronymicAfter = string.Empty;

            if (patronymic.StartsWith("Ибн"))
            {
                patronymicBefore = "ибн ";
                patronymic = patronymic.Substring(4);
            }

            if (patronymic.EndsWith("-оглы") || patronymic.EndsWith("-кызы"))
            {
                patronymicAfter = patronymic.Substring(patronymic.Length - 5);
                patronymic = patronymic.Substring(0, patronymic.Length - 5);
            }

            if (patronymic.StartsWith("Оглы") || patronymic.StartsWith("Кызы") ||
                patronymic.EndsWith("оглы", StringComparison.CurrentCultureIgnoreCase) || patronymic.EndsWith("кызы", StringComparison.CurrentCultureIgnoreCase))
            {
                patronymicAfter = patronymic.Substring(patronymic.Length - 4);
                patronymic = patronymic.Substring(0, patronymic.Length - 4);
            }

            if (caseNumber < 1 || caseNumber > 6)
            {
                caseNumber = 1;
            }

            if (gender < 0 || gender > 2)
            {
                gender = 0;
            }

            if (gender == 0)
            {
                gender = patronymic.EndsWith("на") || (patronymicAfter != string.Empty && patronymicAfter.EndsWith("кызы", StringComparison.CurrentCultureIgnoreCase)) ? 2 : 1;
            }

            isFeminine = (gender == 2);

            if (_lastNamesCache.ContainsKey(surname))
            {
                surnameNew = _lastNamesCache[surname].GetWordForm((WordsForms)caseNumber);
                if (!string.IsNullOrWhiteSpace(surnameNew))
                    surname = surnameNew;
            }
            else
            {
                surnameOld = surname;
                surnameNew = string.Empty;
                index = surnameOld.IndexOf("-");

                string temp = null;
                while (index > 0)
                {
                    temp = ProperCase(surnameOld.Substring(0, index));
                    surnameNew = surnameNew + DeclineSurname(temp, caseNumber, isFeminine) + "-";
                    surnameOld = surnameOld.Substring(index + 1);
                    index = surnameOld.IndexOf("-");
                }

                temp = ProperCase(surnameOld);
                surnameNew = surnameNew + DeclineSurname(temp, caseNumber, isFeminine);
                surname = surnameNew;
            }

            name = DiclineName(name, caseNumber, isFeminine, shorten);
            patronymic = DeclinePatronymic(patronymic, caseNumber, patronymicAfter, isFeminine, shorten);

            if (!shorten)
            {
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(patronymicAfter, "оглы", CompareOptions.IgnoreCase) >= 0 ||
                    CultureInfo.InvariantCulture.CompareInfo.IndexOf(patronymicAfter, "кызы", CompareOptions.IgnoreCase) >= 0)
                {
                    patronymicAfter = patronymicAfter.ToLower();
                }
                patronymic = patronymicBefore + patronymic + patronymicAfter;
            }

            return new[] { surname, name, patronymic };
        }

        /// <summary>
        /// Склонение ФИО в указанном падеже
        /// </summary>
        /// <param name="fio">ФИО</param>
        /// <param name="caseNumber"></param>
        /// <param name="gender">Пол</param>
        /// <param name="shorten"></param>
        /// <returns></returns>
        private static string Decline(string fio, int caseNumber = 1, int gender = 0, bool shorten = false)
        {
            string strF = null;
            string strI = null;
            string strO = null;
            string str1 = null;
            string str2 = null;
            string str3 = null;
            int iInd = 0;

            fio = fio.Trim();
            iInd = fio.IndexOf(" ");

            if (iInd > 0)
            {
                str1 = fio.Substring(0, iInd).Trim().ToLower();
                fio = fio.Substring(iInd).Trim();

                iInd = fio.IndexOf(" ");

                if (iInd > 0)
                {
                    str2 = fio.Substring(0, iInd).Trim().ToLower();
                    str3 = fio.Substring(iInd).Trim().ToLower();
                }
                else
                {
                    str2 = fio.Trim().ToLower();
                }
            }
            else
            {
                str1 = fio.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(str3))
            {
                if (str2.EndsWith("ич") || str2.EndsWith("вна") || str2.EndsWith("чна"))
                {
                    strF = ProperCase(str3);
                    strI = ProperCase(str1);
                    strO = ProperCase(str2);
                }
                else
                {
                    strF = ProperCase(str1);
                    strI = ProperCase(str2);
                    strO = ProperCase(str3);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    if (str2.EndsWith("ич") || str2.EndsWith("вна") || str2.EndsWith("чна"))
                    {
                        strI = ProperCase(str1);
                        strO = ProperCase(str2);
                    }
                    else
                    {
                        strF = ProperCase(str1);
                        strI = ProperCase(str2);
                    }
                }
                else
                {
                    strF = ProperCase(str1);
                }
            }

            return string.Join(" ", Decline(strF, strI, strO, caseNumber, gender, shorten));
        }

        #region Склонение фамилии

        private static string DeclineSurname(string surname, int caseNumber, bool isFeminine)
        {
            string result = surname;

            if (surname.Length <= 1 || caseNumber < 2 || caseNumber > 6)
            {
                result = surname;
                return result;
            }

            switch (caseNumber)
            {
                case 2:
                    result = DeclineSurnameGenitive(surname, isFeminine);
                    break;

                case 3:
                    result = DeclineSurnameDative(surname, isFeminine);
                    break;

                case 4:
                    result = DeclineSurnameAccusative(surname, isFeminine);
                    break;

                case 5:
                    result = DeclineSurnameInstrumental(surname, isFeminine);
                    break;

                case 6:
                    result = DeclineSurnamePrepositional(surname, isFeminine);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclineSurnameGenitive(string surname, bool isFeminine)
        {
            string temp = surname;
            string end = null;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "его");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьца");
                        break;
                    case "нок":
                        surname = SetEnd(surname, "нка");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "га":
                case "жа":
                case "ка":
                case "ха":
                case "ча":
                case "ша":
                case "ща":
                    surname = SetEnd(surname, 1, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 1, "ка");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ька");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "ца");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ого");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ья");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "ея");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, 2, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ы");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "и");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "а";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "а";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "я");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ы");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "и");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclineSurnameDative(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "ему");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьцу");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "ия":
                    surname = SetEnd(surname, 1, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 2, "ку");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьку");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "цу");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ому");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ью");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "ею");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, 2, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, 2, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "е");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "у";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "у";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "ю");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "е");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclineSurnameAccusative(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, 2, "его");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьца");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, "у");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ую");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, "ка");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ька");
                        break;
                    case "ец":
                        surname = SetEnd(surname, "ца");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, 2, "ого");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, "ья");
                        }
                        else
                        {
                            surname = SetEnd(surname, "ея");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ую");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "юю");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "у");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "ю");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "а";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "а";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, "я");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "у");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "ю");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclineSurnameInstrumental(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "лец":
                        surname = SetEnd(surname, 2, "ьцом");
                        break;
                    case "бец":
                        surname = SetEnd(surname, 2, "цем");
                        break;
                    case "кой":
                        surname = SetEnd(surname, "им");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "еца":
                    case "ица":
                    case "аца":
                    case "ьца":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, 2, "ком");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьком");
                        break;
                    case "ец":
                        surname = SetEnd(surname, 2, "цом");
                        break;
                    case "ий":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "им");
                        }
                        break;
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "ым");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, 2, "ьем");
                        }
                        else
                        {
                            surname = SetEnd(surname, 2, "еем");
                        }
                        break;
                    case "оч":
                    case "ич":
                    case "иц":
                    case "ьц":
                    case "ьш":
                    case "еш":
                    case "ыш":
                    case "яц":
                        surname = surname + "ем";
                        break;
                    case "ин":
                    case "ын":
                    case "ен":
                    case "эн":
                    case "ов":
                    case "ев":
                    case "ёв":
                    case "ун":
                        if (surname.ToLower() != "дарвин" && surname.ToLower() != "франклин" && surname.ToLower() != "чаплин" && surname.ToLower() != "грин")
                        {
                            surname = surname + "ым";
                        }
                        break;
                    case "жа":
                    case "ца":
                    case "ча":
                    case "ша":
                    case "ща":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ой");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                        surname = surname + "ом";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "ом";
                        }
                        break;
                    case "щ":
                        surname = surname + "ем";
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, 1, "ем");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, 1, "ой");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, 1, "ей");
                        break;
                }
            }

            return surname;
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclineSurnamePrepositional(string surname, bool isFeminine)
        {
            string temp = surname;
            string end;

            end = SubstringRight(surname, 3);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "жий":
                    case "ний":
                    case "ций":
                    case "чий":
                    case "ший":
                    case "щий":
                        surname = SetEnd(surname, "ем");
                        break;
                    case "лец":
                        surname = SetEnd(surname, 2, "ьце");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ова":
                    case "ева":
                    case "ина":
                    case "ына":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                    case "жая":
                    case "цая":
                    case "чая":
                    case "шая":
                    case "щая":
                        surname = SetEnd(surname, "ей");
                        break;
                    case "ска":
                    case "цка":
                        surname = SetEnd(surname, 1, "ой");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 2);

            switch (end)
            {
                case "ия":
                    surname = SetEnd(surname, "и");
                    break;
            }

            if (surname != temp)
            {
                return surname;
            }

            if (!isFeminine)
            {
                switch (end)
                {
                    case "ок":
                        surname = SetEnd(surname, "ке");
                        break;
                    case "ёк":
                    case "ек":
                        surname = SetEnd(surname, 2, "ьке");
                        break;
                    case "ец":
                        surname = SetEnd(surname, "це");
                        break;
                    case "ий":
                    case "ый":
                    case "ой":
                        if (surname.Length > 4)
                        {
                            surname = SetEnd(surname, "ом");
                        }
                        break;
                    case "ей":
                        if (surname.ToLower() == "соловей" || surname.ToLower() == "воробей")
                        {
                            surname = SetEnd(surname, "ье");
                        }
                        else
                        {
                            surname = SetEnd(surname, "ее");
                        }
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "ая":
                        surname = SetEnd(surname, "ой");
                        break;
                    case "яя":
                        surname = SetEnd(surname, "ей");
                        break;
                }
            }

            if (surname != temp)
            {
                return surname;
            }

            end = SubstringRight(surname, 1);

            if (!isFeminine)
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "е");
                        break;
                    case "б":
                    case "в":
                    case "г":
                    case "д":
                    case "ж":
                    case "з":
                    case "к":
                    case "л":
                    case "м":
                    case "н":
                    case "п":
                    case "р":
                    case "с":
                    case "т":
                    case "ф":
                    case "ц":
                    case "ч":
                    case "ш":
                    case "щ":
                        surname = surname + "е";
                        break;
                    case "х":
                        if (!surname.EndsWith("их") && !surname.EndsWith("ых"))
                        {
                            surname = surname + "е";
                        }
                        break;
                    case "ь":
                    case "й":
                        surname = SetEnd(surname, "е");
                        break;
                }
            }
            else
            {
                switch (end)
                {
                    case "а":
                        switch (surname.Substring(surname.Length - 2, 1))
                        {
                            case "а":
                            case "е":
                            case "ё":
                            case "и":
                            case "о":
                            case "у":
                            case "э":
                            case "ы":
                            case "ю":
                            case "я":
                                break;
                            default:
                                surname = SetEnd(surname, "е");
                                break;
                        }
                        break;
                    case "я":
                        surname = SetEnd(surname, "е");
                        break;
                }
            }

            return surname;
        }

        #endregion

        #region Склонение имени

        /// <summary>
        /// Склонение имени
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="caseNumber">Вид падежа</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <param name="shorten">Признак необходимости сокращенного варианта имени (иницал)</param>
        /// <returns>Склоненное имя</returns>
        private static string DiclineName(string name, int caseNumber, bool isFeminine, bool shorten)
        {
            if (name == null)
                return string.Empty;

            string[] nameParts = name.Split('-');

            for (int index = 0; index < nameParts.Length; index++)
            {
                string namePart = nameParts[index];
                if (namePart.Length <= 1 || namePart.EndsWith("."))
                    continue;

                if (shorten)
                {
                    namePart = namePart.Substring(0, 1) + ".";
                }
                else if (_firstNamesCache.ContainsKey(namePart) &&
                         !string.IsNullOrWhiteSpace(_firstNamesCache[namePart].GetWordForm((WordsForms) caseNumber)))
                {
                    namePart = _firstNamesCache[namePart].GetWordForm((WordsForms) caseNumber);
                }
                else
                {
                    switch (caseNumber)
                    {
                        case 2:
                            namePart = DeclineNameGenitive(namePart, isFeminine);
                            break;

                        case 3:
                            namePart = DeclineNameDative(namePart, isFeminine);
                            break;

                        case 4:
                            namePart = DeclineNameAccusative(namePart, isFeminine);
                            break;

                        case 5:
                            namePart = DeclineNameInstrumental(namePart, isFeminine);
                            break;

                        case 6:
                            namePart = DeclineNamePrepositional(namePart, isFeminine);
                            break;
                    }
                }
                nameParts[index] = namePart;
            }
            name = string.Join("-", nameParts);

            return name;
        }

        /// <summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <returns>Имя в родительсном падеже</returns>
        private static string DeclineNameGenitive(string name, bool isFeminine)
        {
            string temp = name;

            switch (SubstringRight(name, 3).ToLower())
            {
                case "лев":
                    name = SetEnd(name, 2, "ьва");
                    break;
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 2))
                {
                    case "ей":
                    case "ий":
                    case "ай":
                        name = SetEnd(name, "я");
                        break;
                    case "ел":
                        name = SetEnd(name, "ла");
                        break;
                    case "ец":
                        name = SetEnd(name, "ца");
                        break;
                    case "га":
                    case "жа":
                    case "ка":
                    case "ха":
                    case "ча":
                    case "ща":
                        name = SetEnd(name, "и");
                        break;
                }
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 1))
                {
                    case "а":
                        name = SetEnd(name, "ы");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "я":
                        name = SetEnd(name, "и");
                        break;
                    case "ь":
                        name = SetEnd(name, (isFeminine ? "и" : "я"));
                        break;
                    default:
                        if (!isFeminine)
                            name = name + "а";
                        break;
                }
            }

            return name;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <returns>Имя в дательном падеже</returns>
        private static string DeclineNameDative(string name, bool isFeminine)
        {
            string temp = name;

            switch (SubstringRight(name, 3).ToLower())
            {
                case "лев":
                    name = SetEnd(name, 2, "ьву");
                    break;
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 2))
                {
                    case "ей":
                    case "ий":
                    case "ай":
                        name = SetEnd(name, "ю");
                        break;
                    case "ел":
                        name = SetEnd(name, "лу");
                        break;
                    case "ец":
                        name = SetEnd(name, "цу");
                        break;
                    case "ия":
                        name = SetEnd(name, "ии");
                        break;
                }
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 1))
                {
                    case "а":
                    case "я":
                        name = SetEnd(name, "е");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "ь":
                        name = SetEnd(name, (isFeminine ? "и" : "ю"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            name = name + "у";
                        }
                        break;
                }
            }
            return name;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <returns>Имя в винительном падеже</returns>
        private static string DeclineNameAccusative(string name, bool isFeminine)
        {
            string temp = name;

            switch (SubstringRight(name, 3).ToLower())
            {
                case "лев":
                    name = SetEnd(name, 2, "ьва");
                    break;
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 2))
                {
                    case "ей":
                    case "ий":
                    case "ай":
                        name = SetEnd(name, "я");
                        break;
                    case "ел":
                        name = SetEnd(name, "ла");
                        break;
                    case "ец":
                        name = SetEnd(name, "ца");
                        break;
                }
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 1))
                {
                    case "а":
                        name = SetEnd(name, "у");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "я":
                        name = SetEnd(name, "ю");
                        break;
                    case "ь":
                        if (!isFeminine)
                        {
                            name = SetEnd(name, "я");
                        }
                        break;
                    default:
                        if (!isFeminine)
                            name = name + "а";
                        break;
                }
            }

            return name;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <returns>Имя в творительном падеже</returns>
        private static string DeclineNameInstrumental(string name, bool isFeminine)
        {
            string temp = name;

            switch (SubstringRight(name, 3).ToLower())
            {
                case "лев":
                    name = SetEnd(name, 2, "ьвом");
                    break;
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 2))
                {
                    case "ей":
                    case "ий":
                    case "ай":
                        name = SetEnd(name, 1, "ем");
                        break;
                    case "ел":
                        name = SetEnd(name, 2, "лом");
                        break;
                    case "ец":
                        name = SetEnd(name, 2, "цом");
                        break;
                    case "жа":
                    case "ца":
                    case "ча":
                    case "ша":
                    case "ща":
                        name = SetEnd(name, 1, "ей");
                        break;
                }
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 1))
                {
                    case "а":
                        name = SetEnd(name, 1, "ой");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "я":
                        name = SetEnd(name, 1, "ей");
                        break;
                    case "ь":
                        name = SetEnd(name, 1, (isFeminine ? "ью" : "ем"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            name = name + "ом";
                        }
                        break;
                }
            }

            return name;
        }

        /// <summary>
        /// Предложный, О ком? О чем? (думаю)
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <returns>Имя в предложном падеже</returns>
        private static string DeclineNamePrepositional(string name, bool isFeminine)
        {
            string temp = name;

            switch (SubstringRight(name, 3).ToLower())
            {
                case "лев":
                    name = SetEnd(name, 2, "ьве");
                    break;
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 2))
                {
                    case "ей":
                    case "ай":
                        name = SetEnd(name, "е");
                        break;
                    case "ий":
                        name = SetEnd(name, "и");
                        break;
                    case "ел":
                        name = SetEnd(name, "ле");
                        break;
                    case "ец":
                        name = SetEnd(name, "це");
                        break;
                    case "ия":
                        name = SetEnd(name, "ии");
                        break;
                }
            }

            if (name == temp)
            {
                switch (SubstringRight(name, 1))
                {
                    case "а":
                    case "я":
                        name = SetEnd(name, "е");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "ь":
                        name = SetEnd(name, (isFeminine ? "и" : "е"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            name = name + "е";
                        }
                        break;
                }
            }

            return name;
        }

        #endregion

        #region Склонение отчества

        /// <summary>
        /// Склонение отчества
        /// </summary>
        /// <param name="patronymic">Отчество</param>
        /// <param name="caseNumber">Вид падежа</param>
        /// <param name="patronymicAfter"></param>
        /// <param name="isFeminine">Является ли женского рода</param>
        /// <param name="shorten">Признак необходимости сокращенного варианта имени (иницал)</param>
        /// <returns>Склоненное отчество</returns>
        private static string DeclinePatronymic(string patronymic, int caseNumber, string patronymicAfter, bool isFeminine, bool shorten)
        {
            if (patronymic == null)
                return string.Empty;

            string[] patronymicParts = patronymic.Split('-');

            for (int index = 0; index < patronymicParts.Length; index++)
            {
                string patronymicPart = patronymicParts[index];

                if (patronymicPart.Length <= 1 || patronymicPart.EndsWith("."))
                {
                    continue;
                }

                if (shorten)
                {
                    patronymicPart = patronymicPart.Substring(0, 1) + ".";
                }
                else if (_middleNamesCache.ContainsKey(patronymicPart) &&
                         !string.IsNullOrWhiteSpace(_middleNamesCache[patronymicPart].GetWordForm((WordsForms) caseNumber)))
                {
                    patronymicPart = _middleNamesCache[patronymicPart].GetWordForm((WordsForms) caseNumber);
                }
                else
                {
                    switch (caseNumber)
                    {
                        case 2:
                            patronymicPart = DeclinePatronymicGenitive(patronymicPart, patronymicAfter, isFeminine);
                            break;

                        case 3:
                            patronymicPart = DeclinePatronymicDative(patronymicPart, patronymicAfter, isFeminine);
                            break;

                        case 4:
                            patronymicPart = DeclinePatronymicAccusative(patronymicPart, patronymicAfter, isFeminine);
                            break;

                        case 5:
                            patronymicPart = DeclinePatronymicInstrumental(patronymicPart, patronymicAfter,
                                isFeminine);
                            break;

                        case 6:
                            patronymicPart = DeclinePatronymicPrepositional(patronymicPart, patronymicAfter,
                                isFeminine);
                            break;
                    }
                }

                patronymicParts[index] = patronymicPart;
            }

            patronymic = string.Join("-", patronymicParts);

            return patronymic;
        }

        ///<summary>
        /// Родительный, Кого? Чего? (нет)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isFeminine"></param>
        /// <returns></returns>
        private static string DeclinePatronymicGenitive(string patronymic, string patronymicAfter, bool isFeminine)
        {
            if (string.IsNullOrEmpty(patronymicAfter))
            {
                switch (SubstringRight(patronymic, 1))
                {
                    case "а":
                        patronymic = SetEnd(patronymic, "ы");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "я":
                        patronymic = SetEnd(patronymic, "и");
                        break;
                    case "ь":
                        patronymic = SetEnd(patronymic, (isFeminine ? "и" : "я"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            patronymic = patronymic + "а";
                        }
                        break;
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Дательный, Кому? Чему? (дам)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsFeminine"></param>
        /// <param name="Shorten"></param>
        /// <returns></returns>
        private static string DeclinePatronymicDative(string patronymic, string patronymicAfter, bool isFeminine)
        {
            if (string.IsNullOrEmpty(patronymicAfter))
            {
                switch (SubstringRight(patronymic, 1))
                {
                    case "а":
                    case "я":
                        patronymic = SetEnd(patronymic, "е");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "ь":
                        patronymic = SetEnd(patronymic, (isFeminine ? "и" : "ю"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            patronymic = patronymic + "у";
                        }
                        break;
                }
            }
            return patronymic;
        }

        /// <summary>
        /// Винительный, Кого? Что? (вижу)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isFeminine"></param>
        /// <param name="Shorten"></param>
        /// <returns></returns>
        private static string DeclinePatronymicAccusative(string patronymic, string patronymicAfter, bool isFeminine)
        {
            if (string.IsNullOrEmpty(patronymicAfter))
            {
                switch (SubstringRight(patronymic, 1))
                {
                    case "а":
                        patronymic = SetEnd(patronymic, "у");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "я":
                        patronymic = SetEnd(patronymic, "ю");
                        break;
                    case "ь":
                        if (!isFeminine)
                            patronymic = SetEnd(patronymic, "я");
                        break;
                    default:
                        if (!isFeminine)
                            patronymic = patronymic + "а";
                        break;
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isFeminine"></param>
        /// <param name="Shorten"></param>
        /// <returns></returns>
        private static string DeclinePatronymicInstrumental(string patronymic, string patronymicAfter, bool isFeminine)
        {
            if (string.IsNullOrEmpty(patronymicAfter))
            {
                string temp = patronymic;

                switch (SubstringRight(patronymic, 2))
                {
                    case "ич":
                        patronymic = patronymic + (patronymic.ToLower() == "ильич" ? "ом" : "ем");
                        break;
                    case "на":
                        patronymic = SetEnd(patronymic, 2, "ной");
                        break;
                }

                if (patronymic == temp)
                {
                    switch (SubstringRight(patronymic, 1))
                    {
                        case "а":
                            patronymic = SetEnd(patronymic, 1, "ой");
                            break;
                        case "е":
                        case "ё":
                        case "и":
                        case "о":
                        case "у":
                        case "э":
                        case "ю":
                            break;
                        case "я":
                            patronymic = SetEnd(patronymic, 1, "ей");
                            break;
                        case "ь":
                            patronymic = SetEnd(patronymic, 1, (isFeminine ? "ью" : "ем"));
                            break;
                        default:
                            if (!isFeminine)
                            {
                                patronymic = patronymic + "ом";
                            }
                            break;
                    }
                }
            }

            return patronymic;
        }

        /// <summary>
        /// Творительный, Кем? Чем? (горжусь)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isFeminine"></param>
        /// <param name="Shorten"></param>
        /// <returns></returns>
        private static string DeclinePatronymicPrepositional(string patronymic, string patronymicAfter, bool isFeminine)
        {
            if (string.IsNullOrEmpty(patronymicAfter))
            {
                switch (SubstringRight(patronymic, 1))
                {
                    case "а":
                    case "я":
                        patronymic = SetEnd(patronymic, "е");
                        break;
                    case "е":
                    case "ё":
                    case "и":
                    case "о":
                    case "у":
                    case "э":
                    case "ю":
                        break;
                    case "ь":
                        patronymic = SetEnd(patronymic, (isFeminine ? "и" : "е"));
                        break;
                    default:
                        if (!isFeminine)
                        {
                            patronymic = patronymic + "е";
                        }
                        break;
                }
            }

            return patronymic;
        }

        #endregion

        #region Вспомогательные методы

        private static string ProperCase(string val)
        {
            if (val != null)
            {
                val = val.Replace("\uFEFF", string.Empty).Trim(); //ZERO WIDTH NO-BREAK SPACE
            }

            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            val = val.ToLower();

            string[] values = val.Split('-');

            for (int index = 0; index < values.Length; index++)
            {
                string value = values[index];
                if (string.IsNullOrEmpty(value)) 
                    values[index] = string.Empty;
                else
                    values[index] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value);
            }

            return string.Join("-", values);
        }

        private static string SetEnd(string value, string add)
        {
            return SetEnd(value, add.Length, add);
        }

        private static string SetEnd(string value, int cut, string add)
        {
            return value.Substring(0, value.Length - cut) + add;
        }

        private static string SubstringRight(string value, int cut)
        {
            if (cut > value.Length)
                cut = value.Length;

            return value.Substring(value.Length - cut);
        }

        #endregion

        #endregion
    }
}