using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSUtils.LinkExtension
{
    public static class LinkControlByPropertyName
    {
        public static void Link(this object obj, string property, params object[] linkingObjects)
        {
            obj.CreatelinkWithInfo(
                new LinkExtensionHelper.LinkObjBinder(property, linkingObjects));

            foreach (var linkingObject in linkingObjects)
            {
                linkingObject.CreatelinkWithInfo(
                    new LinkExtensionHelper.LinkObjBinder(property, obj));
            }
        }

        public static void LinkOneWay(this object obj, string property, params object[] linkingObjects)
        {
            obj.CreatelinkWithInfo(
                new LinkExtensionHelper.LinkObjBinder(property, linkingObjects));
        }

        public static bool UnLink(this object obj)
        {
            return obj.RemoveLink();
        }
    }

    internal static class LinkExtensionHelper
    {
        static readonly Dictionary<object, List<LinkObjBinder>> LinkInfoTable = new Dictionary<object, List<LinkObjBinder>>();

        internal static List<LinkObjBinder> GetLinkInfo<T>(this T obj)
        {
            List<LinkObjBinder> ret;
            if (!LinkInfoTable.TryGetValue(obj, out ret))
            {
                ret = new List<LinkObjBinder>(); // create&return empty list instead of throwing an exception.
            }
            return ret;
        }

        internal static bool RemoveLink<T>(this T obj)
        {
            var binderObjlist = GetLinkInfo(obj);
            Type t = obj.GetType();

            foreach (var binderObj in binderObjlist)
            {
                // check if property exists
                PropertyInfo propInfo = t.GetProperty(binderObj.Property);
                // get event handler if there is a change event for given property item
                EventInfo evInfo = t.GetEvent(string.Format("{0}Changed", binderObj.Property));

                evInfo.RemoveEventHandler(obj, Delegate.CreateDelegate(evInfo.EventHandlerType, binderObj, "OnEventHandle"));
                foreach (var item in binderObj.LinkedObjects)
                {
                    var binderObjLinkedList = GetLinkInfo(item);
                    foreach (var binderObjLinked in binderObjLinkedList)
                    {
                        // get event handler if there is a change event for given property item
                        EventInfo evInfoLinked = item.GetType().GetEvent(string.Format("{0}Changed", binderObjLinked.Property));

                        evInfoLinked.RemoveEventHandler(item, Delegate.CreateDelegate(evInfoLinked.EventHandlerType, binderObjLinked, "OnEventHandle"));
                    }
                    bool result = LinkInfoTable.Remove(item);
                    Console.WriteLine("{0}", result);
                }
            }

            bool res = LinkInfoTable.Remove(obj);
            return res;
        }

        internal static void CreatelinkWithInfo<T>(this T obj, LinkObjBinder linkInfo)
        {
            List<LinkObjBinder> LinkObjBinderlist;
            // obtain or create list for given object
            if (!LinkInfoTable.TryGetValue(obj, out LinkObjBinderlist)) LinkInfoTable.Add(obj, LinkObjBinderlist = new List<LinkObjBinder>());

            Type t = obj.GetType();

            // check if property exists
            PropertyInfo propInfo = t.GetProperty(linkInfo.Property);
            // get event handler if there is a change event for given property item
            EventInfo evInfo = t.GetEvent(string.Format("{0}Changed", linkInfo.Property));

            evInfo.AddEventHandler(obj, Delegate.CreateDelegate(evInfo.EventHandlerType, linkInfo, "OnEventHandle"));
            // then add new one
            LinkObjBinderlist.Add(linkInfo);
        }

        internal class LinkObjBinder
        {
            public object[] LinkedObjects { get; internal set; }
            public string Property { get; internal set; }

            public LinkObjBinder(string Property, params object[] LinkObjects)
            {
                this.Property = Property;
                this.LinkedObjects = LinkObjects;
            }

            public void OnEventHandle(object sender, EventArgs e)
            {
                Type t = sender.GetType();
                // check if property exists
                PropertyInfo propInfoLinker = t.GetProperty(Property);

                foreach (var linkedObject in LinkedObjects)
                {
                    // get linked object property instance
                    PropertyInfo propInfoLinked = linkedObject.GetType().GetProperty(Property);
                    // set new value into retreived property instance of linked object
                    propInfoLinked.SetValue(linkedObject, propInfoLinker.GetValue(sender));
                }
            }
        }
    }
}
