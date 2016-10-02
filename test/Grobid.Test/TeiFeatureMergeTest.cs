using System;
using System.Linq;
using System.Xml.Linq;

using ApprovalTests;
using Xunit;

using Grobid.NET;


namespace Grobid.Test
{
    public class TeiFeatureMergeTest
    {
        [Fact]
        public void TeiFeatureMerge00()
        {
            var testSubject = new TeiFeatureMerge();
            var teiFeatureFactory = new TeiFeatureFactory();

            var teiFeatures = teiFeatureFactory.Create(XDocument.Parse(TeiFeatureMergeTest.Tei));
            var featureRows = TeiFeatureMergeTest.FeatureRows
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(x => x.Trim())
                .Select(FeatureRow.Parse)
                .ToArray();

            var aligned = testSubject.Merge(teiFeatures, featureRows);

            // There are a three differences between this code and Grobid's code.  The
            // following lines were found in Grobid's output, but not not my output.
            //
            // WHY?
            // 
            // IL	<note>	<address>
            // 61801 < note >  < address >
            // ,	< note >  < address >
            // USA < note >  < address >
            // ,	< email > < email >
            // [29] kaleg@cs	<email>	<email>
            // .	<email>	<email>
            // uiuc	<email>	<email>
            // .	<email>	<email>
            // edu	<email>	<email>
            // Abstract	I-<abstract>	I-<abstract>
            // --
            // they <abstract>	<abstract>
            // are<abstract>	<abstract>
            // often<abstract>	<abstract>
            // [124] diicult	<abstract>	<abstract>
            // to <abstract>	<abstract>
            // develop<abstract>	<abstract>
            // and<abstract>	<abstract>
            // debug<abstract>	<abstract>
            // --
            // which <abstract>	<abstract>
            // is	<abstract>	<abstract>
            // called<abstract>	<abstract>
            // [150] -	<abstract>	<abstract>
            // expect <abstract>	<abstract>
            // ,	<abstract>	<abstract>
            // that<abstract>	<abstract>
            // replaces<abstract>	<abstract>
            Approvals.VerifyAll(aligned, x => $"{x.Value} {x.Expected} {x.Actual}");
        }

        private const string Tei = @"<tei>
  <teiHeader>
    <fileDesc xml:id='306'/>
  </teiHeader>
  <text xml:lang='en'>
    <front>
      <docTitle>
	<titlePart type='main'>Tolerating Latency with Dagger <lb/></titlePart>
      </docTitle>
      <byline><docAuthor>Attila Gursoy and L.V.Kale <lb/></docAuthor></byline>
      <byline><affiliation>Department of Computer Science <lb/>University of Illinois at Urbana-Champaign <lb/></affiliation></byline>
      <note type='other'>Urbana IL 61801, USA <lb/></note>
      <email>{fgursoy,kaleg}@cs.uiuc.edu <lb/></email>
      <div type='abstract'>Abstract <lb/>The communication latency is a major issue that must be dealt with in parallel <lb/>computing. The parallel computation model therefore must provide the ability to tolerate <lb/>such latencies. Communication using blocking receives is the commonly used mechanism <lb/>in parallel programming today. Message driven execution is an alternate mechanism <lb/>which does not use receive style statements at all. The message driven execution style <lb/>promotes the overlap of computation and communication: Programs written in this style <lb/>exhibit increased latency tolerance. However, they are often difficult to develop and <lb/>debug. We present a coordination language called Dagger to alleviate this problem. The <lb/>language has a mechanism which is called expect, that replaces the receive statement. <lb/>It has been implemented in the Charm parallel programming system, and runs programs <lb/>portably on a variety of parallel machines. <lb/></div>
      <div type='intro'>1. INTRODUCTION<lb/></div>
    </front>
  </text>
</tei>";

        private const string FeatureRows =
            @"Tolerating tolerating T To Tol Tole g ng ing ting BLOCKSTART LINESTART NEWFONT HIGHERFONT 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<title>
Latency latency L La Lat Late y cy ncy ency BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <title>
with with w wi wit with h th ith with BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <title>
Dagger dagger D Da Dag Dagg r er ger gger BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <title>
Attila attila A At Att Atti a la ila tila BLOCKSTART LINESTART SAMEFONT LOWERFONT 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<author>
and and a an and and d nd and and BLOCKIN LINEIN SAMEFONT LOWERFONT 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <author>
L l L L L L L L L L BLOCKIN LINEIN SAMEFONT HIGHERFONT 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <author>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <author>
V v V V V V V V V V BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <author>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <author>
Kale kale K Ka Kal Kale e le ale Kale BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <author>
Department department D De Dep Depa t nt ent ment BLOCKSTART LINESTART SAMEFONT LOWERFONT 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<affiliation>
of of o of of of f of of of BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
Computer computer C Co Com Comp r er ter uter BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
Science science S Sc Sci Scie e ce nce ence BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
University university U Un Uni Univ y ty ity sity BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
of of o of of of f of of of BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
Illinois illinois I Il Ill Illi s is ois nois BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
at at a at at at t at at at BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
Urbana urbana U Ur Urb Urba a na ana bana BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
- - - - - - - - - - BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 1 HYPHEN 0 0 <affiliation>
Champaign champaign C Ch Cha Cham n gn ign aign BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <affiliation>
Urbana urbana U Ur Urb Urba a na ana bana BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<address>
IL il I IL IL IL L IL IL IL BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <address>
61801 61801 6 61 618 6180 1 01 801 1801 BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS ALLDIGIT 0 0 0 0 0 1 0 0 0 0 NOPUNCT 0 0 <address>
, , , , , , , , , , BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 COMMA 0 0 <address>
USA usa U US USA USA A SA USA USA BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <address>
, , , , , , , , , , BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 COMMA 0 0 <email>
kaleg@cs kaleg@cs k ka kal kale s cs @cs g@cs BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <email>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <email>
uiuc uiuc u ui uiu uiuc c uc iuc uiuc BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <email>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <email>
edu edu e ed edu edu u du edu edu BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <email>
Abstract abstract A Ab Abs Abst t ct act ract BLOCKSTART LINESTART SAMEFONT HIGHERFONT 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<abstract>
The the T Th The The e he The The BLOCKSTART LINESTART SAMEFONT LOWERFONT 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
communication communication c co com comm n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 1 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
latency latency l la lat late y cy ncy ency BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
is is i is is is s is is is BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
a a a a a a a a a a BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 1 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
major major m ma maj majo r or jor ajor BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 1 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
issue issue i is iss issu e ue sue ssue BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
that that t th tha that t at hat that BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
must must m mu mus must t st ust must BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
be be b be be be e be be be BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
dealt dealt d de dea deal t lt alt ealt BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
with with w wi wit with h th ith with BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
in in i in in in n in in in BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
parallel parallel p pa par para l el lel llel BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
computing computing c co com comp g ng ing ting BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
The the T Th The The e he The The BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
parallel parallel p pa par para l el lel llel BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
computation computation c co com comp n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
model model m mo mod mode l el del odel BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
therefore therefore t th the ther e re ore fore BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
must must m mu mus must t st ust must BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
provide provide p pr pro prov e de ide vide BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
the the t th the the e he the the BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
ability ability a ab abi abil y ty ity lity BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
to to t to to to o to to to BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
tolerate tolerate t to tol tole e te ate rate BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
such such s su suc such h ch uch such BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
latencies latencies l la lat late s es ies cies BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
Communication communication C Co Com Comm n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 1 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
using using u us usi usin g ng ing sing BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
blocking blocking b bl blo bloc g ng ing king BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
receives receives r re rec rece s es ves ives BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
is is i is is is s is is is BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
the the t th the the e he the the BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
commonly commonly c co com comm y ly nly only BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
used used u us use used d ed sed used BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
mechanism mechanism m me mec mech m sm ism nism BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
in in i in in in n in in in BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
parallel parallel p pa par para l el lel llel BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
programming programming p pr pro prog g ng ing ming BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
today today t to tod toda y ay day oday BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
Message message M Me Mes Mess e ge age sage BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
driven driven d dr dri driv n en ven iven BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
execution execution e ex exe exec n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
is is i is is is s is is is BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
an an a an an an n an an an BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
alternate alternate a al alt alte e te ate nate BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
mechanism mechanism m me mec mech m sm ism nism BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
which which w wh whi whic h ch ich hich BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
does does d do doe does s es oes does BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
not not n no not not t ot not not BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
use use u us use use e se use use BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
receive receive r re rec rece e ve ive eive BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
style style s st sty styl e le yle tyle BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
statements statements s st sta stat s ts nts ents BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
at at a at at at t at at at BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
all all a al all all l ll all all BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
The the T Th The The e he The The BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
message message m me mes mess e ge age sage BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
driven driven d dr dri driv n en ven iven BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
execution execution e ex exe exec n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
style style s st sty styl e le yle tyle BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
promotes promotes p pr pro prom s es tes otes BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
the the t th the the e he the the BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
overlap overlap o ov ove over p ap lap rlap BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
of of o of of of f of of of BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
computation computation c co com comp n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
and and a an and and d nd and and BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
communication communication c co com comm n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 1 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
: : : : : : : : : : BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 PUNCT 0 0 <abstract>
Programs programs P Pr Pro Prog s ms ams rams BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
written written w wr wri writ n en ten tten BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
in in i in in in n in in in BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
this this t th thi this s is his this BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
style style s st sty styl e le yle tyle BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
exhibit exhibit e ex exh exhi t it bit ibit BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
increased increased i in inc incr d ed sed ased BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
latency latency l la lat late y cy ncy ency BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
tolerance tolerance t to tol tole e ce nce ance BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
However however H Ho How Howe r er ver ever BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
, , , , , , , , , , BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 COMMA 0 0 <abstract>
they they t th the they y ey hey they BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
are are a ar are are e re are are BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
often often o of oft ofte n en ten ften BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
diicult diicult d di dii diic t lt ult cult BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
to to t to to to o to to to BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
develop develop d de dev deve p op lop elop BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
and and a an and and d nd and and BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
debug debug d de deb debu g ug bug ebug BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
We we W We We We e We We We BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
present present p pr pre pres t nt ent sent BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
a a a a a a a a a a BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 1 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
coordination coordination c co coo coor n on ion tion BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
language language l la lan lang e ge age uage BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
called called c ca cal call d ed led lled BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
Dagger dagger D Da Dag Dagg r er ger gger BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
to to t to to to o to to to BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
alleviate alleviate a al all alle e te ate iate BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
this this t th thi this s is his this BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
problem problem p pr pro prob m em lem blem BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
The the T Th The The e he The The BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
language language l la lan lang e ge age uage BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
has has h ha has has s as has has BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
a a a a a a a a a a BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 1 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
mechanism mechanism m me mec mech m sm ism nism BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
which which w wh whi whic h ch ich hich BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
is is i is is is s is is is BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
called called c ca cal call d ed led lled BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
- - - - - - - - - - BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 1 HYPHEN 0 0 <abstract>
expect expect e ex exp expe t ct ect pect BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
, , , , , , , , , , BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 COMMA 0 0 <abstract>
that that t th tha that t at hat that BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
replaces replaces r re rep repl s es ces aces BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
the the t th the the e he the the BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
receive receive r re rec rece e ve ive eive BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
statement statement s st sta stat t nt ent ment BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
It it I It It It t It It It BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
has has h ha has has s as has has BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
been been b be bee been n en een been BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
implemented implemented i im imp impl d ed ted nted BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
in in i in in in n in in in BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
the the t th the the e he the the BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
Charm charm C Ch Cha Char m rm arm harm BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 INITCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
parallel parallel p pa par para l el lel llel BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
programming programming p pr pro prog g ng ing ming BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
system system s sy sys syst m em tem stem BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
, , , , , , , , , , BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 COMMA 0 0 <abstract>
and and a an and and d nd and and BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
runs runs r ru run runs s ns uns runs BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
programs programs p pr pro prog s ms ams rams BLOCKIN LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
portably portably p po por port y ly bly ably BLOCKIN LINESTART SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
on on o on on on n on on on BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
a a a a a a a a a a BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 1 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
variety variety v va var vari y ty ety iety BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
of of o of of of f of of of BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
parallel parallel p pa par para l el lel llel BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
machines machines m ma mac mach s es nes ines BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 NOCAPS NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <abstract>
. . . . . . . . . . BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <abstract>
1 1 1 1 1 1 1 1 1 1 BLOCKSTART LINESTART SAMEFONT HIGHERFONT 0 0 0 NOCAPS ALLDIGIT 1 0 0 0 0 0 0 0 0 0 NOPUNCT 0 0 I-<intro>
. . . . . . . . . . BLOCKIN LINEIN SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 1 0 0 0 0 0 0 0 0 0 DOT 0 0 <intro>
INTRODUCTION introduction I IN INT INTR N ON ION TION BLOCKEND LINEEND SAMEFONT SAMEFONTSIZE 0 0 0 ALLCAP NODIGIT 0 0 1 0 0 0 0 0 0 0 NOPUNCT 0 0 <intro>";
    }
}
