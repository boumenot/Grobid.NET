using System;
using System.Collections.Generic;
using System.Linq;

using Grobid.NET.Contract;

// XXX: this is a work in progress, not much to see here yet.

namespace Grobid.NET.Feature.Date
{
    // This is a rough sketch of my intention...  The names are purposefully
    // ridiculuous because they have no meaning.
    // 
    // Given an X we want to extract metadata about it.  Extracting metadata
    // requires several different concepts, but from the end user's
    // persepective they supply X and get a type representing the metadata
    // for X.  This is the contract.
    //
    // interface IModel<T> {
    //     T Create(BlockState[] blockState);
    // }
    //
    // In reality the user cannot supply an arbitrary X, they have to supply
    // a PDF document.  PDF document get parsed to BlockState[].
    //
    // ## AppleSauceFeatureVectorFactory
    // Every block state is converted to a feature vector.  A feature vector
    // provides metadata about each block state.  e.g. does the block state
    // represent a number, or is it all capital letters.  Feature vectors are
    // determined ahead of time, and without user's input.  Better features
    // lead to better extraction.
    //
    // ## AppleSauceFormatter
    // A feature vector is converted to a list of key/value pairs.  (This step
    // may be unnecessary.)
    //
    // ## WapitiFeatureVectorTransformer
    // key/value pairs are formatted for processing by the target CRP library.
    // In the case of Wapiti feature values are separated by a space.  A feature
    // vector's key/value pairs are aggregated, and separated by a newline.  The
    // feature names are not kept for Wapiti.
    //
    // ## AppleSauceLabeler
    // The key/value pair "document" is then processed by the labeler (Wapiti).
    //
    // Wapiti returns a nearly identical document as was supplied with the addition
    // of labels added to each line.  Labels are of the form I-<label> or <label>
    // and appear at the end of a line.  They are the last field, and are separated
    // by a '\t' as opposed to a space ' ' in the case of Wapiti.
    //
    // The labeled document is parsed to extract the original block state value,
    // Wapiti's assigned label, and if it is a starting label or not, i.e. I-<label>
    // vs. <label>.  Each line is extracted to a FeatureRow, which represents the
    // aforementioned state.
    //
    // ## AppleSauceModelFactory
    // The feature rows are transformed to a model.  The model is a type
    // representing metadata extracted from the PDF.

    public sealed class WapitiFeatureVectorTransformer
    {
        public string Transform(KeyValuePair<string, string>[] features)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauce : IModel<AppleSauceModel>
    {
        private AppleSauceFormatter formatter;
        private AppleSauceFeatureVectorFactory factory;
        private WapitiFeatureVectorTransformer transformer;
        private AppleSauceLabeler labeler;
        private AppleSauceModelFactory modelFactory;

        public AppleSauce()
        {
            this.factory = new AppleSauceFeatureVectorFactory();
            this.modelFactory = new AppleSauceModelFactory();
            this.formatter = new AppleSauceFormatter();
            this.transformer = new WapitiFeatureVectorTransformer();
            this.labeler = new AppleSauceLabeler();
        }

        public AppleSauceModel Create(BlockState[] blockStates)
        {
            var featureVectorLines = blockStates
                .Select(x => this.factory.Create(x))
                .Select(x => this.formatter.Format(x))
                .Select(x => this.transformer.Transform(x));

            var featureVectorDocument = String.Join(Environment.NewLine, featureVectorLines);
            var featureRows = this.labeler.Label(featureVectorDocument);
            var model = this.modelFactory.Create(featureRows);
            return model;
        }
    }

    // 1. PDF Blocks              -> AppleSauceFeatureVector   # AppleSauceFactory
    // 2. AppleSauceFeatureVector -> string[]     # AppleSauceFormatter
    // 2.a. string[] -> IEnumerable<string[]>
    // 3. IEnumerable<string[]>   -> FeatureRow[] # AppleSauceLabeler
    // 4. FeatureRow[]            -> AppleSauceModel

    public class AppleSauceLabeler
    {
        public FeatureRow[] Label(string document)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFormatter
    {
        public KeyValuePair<string, string>[] Format(AppleSauceFeatureVector appleSauce)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFeatureVectorFactory : IFeatureVectorFactory<AppleSauceFeatureVector>
    {
        public AppleSauceFeatureVector Create(BlockState blockState)
        {
            throw new NotImplementedException();
        }
    }

    public class AppleSauceFeatureVector
    {
    }

    public class AppleSauceModelFactory
    {
        public AppleSauceModel Create(FeatureRow[] featureRows)
        {
            throw new NotImplementedException();
        }
    }

    public class AuthorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Affiliation { get; set; }
    }

    public class AppleSauceModel
    {
        public string Title { get; set; }
        public AuthorModel[] Authors { get; set; }
        public string[] Keywords { get; set; }
        public string Abstract { get; set; }
    }
}
