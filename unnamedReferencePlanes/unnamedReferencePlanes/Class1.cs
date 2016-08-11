//Unnamed Reference Planes
//original example created by: Jeremy Tammik
//Copyright (c) 2014-2016, Autodesk Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using BuildingCoder;
using System.Drawing;




using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.Utility;
using Autodesk.Revit.Exceptions;


namespace unnamedRefPlanes
{
 
    [Transaction(TransactionMode.Manual)]





    class Command : IExternalCommand
        
    {
        static int _i = 0;

        bool DeleteIfNotHosing(ReferencePlane rp)
        {
            bool rc = false;

            Document doc = rp.Document;

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Delete ReferencePlane"
                    + (++_i).ToString());
                try
                {
                    ICollection<ElementId> ids = doc.Delete(
                        rp.Id);

                    tx.Commit();
                    rc = true;
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException)
                {
                    tx.RollBack();
                }
            }
            return rc;
        }

        public Result Execute(ExternalCommandData commandData, 
            ref string message, 
            ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            

            BuiltInParameter bip = BuiltInParameter.DATUM_TEXT;

            ParameterValueProvider provider = new ParameterValueProvider(new ElementId(bip));

            FilterStringRuleEvaluator evaluator = new FilterStringEquals();

            FilterStringRule rule = new FilterStringRule(
                provider, evaluator, "", false);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            FilteredElementCollector col = new FilteredElementCollector(doc)
                .OfClass(typeof(ReferencePlane)).WherePasses(filter);

            int n = 0;
            int nDeleted = 0;

            ICollection<ElementId> ids = col.ToElementIds();

            n = ids.Count();

            if(0 < n)
            {
                //Util.PluralSuffix from JT Gethub
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start( string.Format(
                        "Delete {0} ReferencePlane{1}",
                        n, Util.PluralSuffix (n)));
                    

                    List<ElementId> ids2 = new List<ElementId>(ids);

                    foreach(ElementId id in ids2)
                    {
                        try
                        {
                            ICollection<ElementId> ids3 = doc.Delete(
                                id);
                            nDeleted += ids3.Count;
                        }
                        catch(Autodesk.Revit.Exceptions.ArgumentException)
                        {

                        }
                    }
                    tx.Commit();
                }
            }
            Util.InfoMsg(string.Format(
                "{0} unnamed reference plane{1} examined,"
                + "{2} element {3} in total were deleted.",
                n, Util.PluralSuffix(n),
                nDeleted, Util.PluralSuffix(nDeleted)));

            return Result.Succeeded;
        }
    }
}

