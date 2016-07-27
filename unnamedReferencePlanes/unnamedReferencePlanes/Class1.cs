using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace unnamedReferencePlanes
{
    [Transaction (TransactionMode.Manual)]

    class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ElementCategoryFilter categoryfilter =
                new ElementCategoryFilter(BuiltInCategory.OST_ReferenceLines);
            FilteredElementCollector collector = new FilteredElementCollector(doc).WherePasses(categoryfilter);

            //var query = from element in collector
            //            where element.Name != null;

            //ElementParameterFilter refName = new ElementParameterFilter(
            //BuiltInParameter bip = new BuiltInParameter.DATUM_TEXT

            //try
            //{
            //        foreach (Element e in collector)
            //        {

            //        }
            throw new NotImplementedException();
        }
    }
      
    }
}
