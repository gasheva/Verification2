﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Verification.type_definer
{
    /**
    * Быстрое пределение типа диаграммы
    */
    static class TypeDefiner
    {
        /**
         * Проверка, что в списке переданных элементов есть хотя бы один с атрибутом равным значению type
         */
        private static bool FindActivePackageEl(XmlNodeList xPackagedList, string type)
        {
            foreach (XmlNode node in xPackagedList)
            {
                var attr = node.Attributes["xsi:type"];
                if (attr == null) continue;
                if (attr.Value.Equals(type))
                    return true;
            }
            return false;
        }

        /**
         * Определение типа диаграммы
         * return тип диаграммы или UNDEF, если тип не определен
         */
        public static EDiagramTypes DefineDiagramType(XmlElement root)
        {
            XmlNodeList xPackagedList;
            try
            {
                xPackagedList = root.GetElementsByTagName("packagedElement");
            }
            catch (NullReferenceException)
            {
                return EDiagramTypes.UNDEF;
            }

            // получение всех packagedElement
            // проверка на AD
            if (FindActivePackageEl(xPackagedList, "uml:Activity"))
                return EDiagramTypes.AD;

            // проверка на UCD
            if (FindActivePackageEl(xPackagedList, "uml:UseCase"))
                return EDiagramTypes.UCD;

            // проверка на CD
            if (FindActivePackageEl(xPackagedList, "uml:Class"))
                return EDiagramTypes.CD;

            return EDiagramTypes.UNDEF;
        }
    }
}

