using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

namespace Grobid.Test.Wapiti
{
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class WapitiTest
    {
        [Fact, Trait("Category", "EndToEnd")]
        public void WapitiEndToEnd()
        {
            var testSubject = global::Wapiti.Wapiti.Load(@"content\models\date\model.wapiti");

            // 10 January 2001
            var lines = new[]
            {
                "10 10 1 10 10 10 0 10 10 10 LINESTART NOCAPS ALLDIGIT 0 0 0 NOPUNCT <date>",
                "January january J Ja Jan Janu y ry ary uary LINEIN INITCAP NODIGIT 0 0 1 NOPUNCT <date>",
                "2001 2001 2 20 200 2001 1 01 001 2001 LINEEND NOCAPS ALLDIGIT 0 1 0 NOPUNCT <date>",
            };

            var result = testSubject.Label(lines);
            Approvals.Verify(result);
        }
    }
}
