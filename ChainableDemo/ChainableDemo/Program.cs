using System;
using System.Linq;

namespace ChainableDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add manufacturers
            var McBurger = Manufacturer.ChainedManufacturer("McBurger", null);
            var Wednesdays = Manufacturer.ChainedManufacturer("Wednesdays", McBurger);
            var OtherMcBurger = Manufacturer.ChainedManufacturer("McBurger", Wednesdays);

            // Add vendors
            var CattleCo = Vendor.ChainedVendor("Cattle Co", null);
            var FrozenWormsLtd = Vendor.ChainedVendor("Frozen Worms Ltd.", CattleCo);

            // Link vendors and manufacturers
            McBurger.AddVendor(CattleCo);
            OtherMcBurger.AddVendor(FrozenWormsLtd);
            Wednesdays.AddVendor(CattleCo);

            ///////////
            // TESTS //
            ///////////

            // OtherMcBurger and McBurger should be same reference
            Assert(ReferenceEquals(McBurger, OtherMcBurger), "Not getting the same address!!");
            Assert(McBurger.HasVendor(FrozenWormsLtd), "Oops, vendor not added correctly");

            // Remove a node
            Wednesdays.UnlinkFromChain();
            Assert(McBurger.GetNodesInChain<Manufacturer>().Count() == 1, "Node not removed!");

        }

        /// <summary>
        /// Throws exception if statement evaluates false
        /// </summary>
        /// <param name="expression">Expression that is expected to be true</param>
        /// <param name="message">Message to show in error</param>
        /// <returns></returns>
        static void Assert(bool expression, string message)
        {
            if (!expression)
            {
                throw new Exception(message);
            }
        }
    }
}
