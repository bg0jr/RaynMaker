using NUnit.Framework;

namespace RaynMaker.Modules.Import.Tests.Html
{
    [TestFixture]
    class HtmlTableTests_WithoutTBody : HtmlTableTestsBase
    {
        protected override string GetHtml()
        {
            return @"
<html>
    <body>
        <table>
            <tr id='row0'>
                <th id='c00'/>
                <th id='c01'>Column 1</th>
                <th id='c02'>Column 2</th>
                <th id='c03'>Column 3</th>
            </tr>
            <tr id='row1'>
                <th id='c10'>Row 1</th>
                <td id='c11'>v 11</td>
                <td id='c12'>v 12</td>
                <td id='c13'>v 13</td>
            </tr>
            <tr id='row2'>
                <th id='c20'>Row 2</th>
                <td id='c21'>v 21</td>
                <td id='c22'>v 22</td>
                <td id='c23'>v 23</td>
            </tr>
            <tr id='row3'>
                <th id='c30'>Row 3</th>
                <td id='c31'>v 31</td>
                <td id='c32'>v 32</td>
                <td id='c33'>v 33</td>
            </tr>
        </table>
    </body>
</html>
";
        }
    }
}
