module Domain =

    type SMSType =
        | Spam
        | Ham

    type ClassifiedSMS 
        = { Type : SMSType; Text : string }

    type Group = 
        { Proportion : float ; TokensToCount : Map< string, float > }

module Parser = 

    open Domain
    open System.IO

    let dataPath = Path.Combine( __SOURCE_DIRECTORY__, "Data", "SMSSpamCollection" )

    let parseSingleLine ( line : string ) =
        let splitLine = line.Split( '\t' )
        let smsType = splitLine.[0]
        let text = splitLine.[1]
        match smsType with
        | "spam" ->
            { Type = Spam; Text = text } 
        | "ham"  -> 
            { Type = Ham; Text = text }
        | _ -> failwith "Not a valid SMS type"

    let extractTypeFromRecord ( record : ClassifiedSMS ) : SMSType =
        record.Type

    let parseAllLines : ClassifiedSMS seq = 
        File.ReadAllLines dataPath
        |> Seq.toList
        |> Seq.map parseSingleLine

module Tokenizer =
    let tokenize ( text : string ) : Set<string> = 
        text.Split(' ')
        |> Set.ofArray

module Summarizer = 

    open Domain

    let createSummary ( group : seq<Set<string>> )
                      ( total : int )
                      ( tokensToUse : Set<string> ) =

        let correctionFactor = 1.0

        let totalSize = 
            float ( group |> Seq.length ) + correctionFactor

        let getProportionalityForToken ( token : string ) : float =
            let countOfTokenInGroup = 
                group 
                |> Seq.filter( Set.contains( token )) 
                |> Seq.length 
                |> float 

            ( countOfTokenInGroup + correctionFactor ) / ( totalSize + correctionFactor ) 

        let createMap : Map<string, float> =
            tokensToUse 
            |> Set.map(fun t -> t, getProportionalityForToken t ) 
            |> Map.ofSeq
        {
            Proportion = ( totalSize - correctionFactor ) / float( total ) 
            TokensToCount = createMap
        }

module Classifier = 

    open Domain
    open Parser
    open Tokenizer

    let score ( setOfTokens : Set< string > ) ( group : Group ) =

        let scoreToken ( token : string ) : float = 
            if group.TokensToCount.ContainsKey token then 
                log group.TokensToCount.[ token ]  
            else
                0.0

        log group.Proportion + 
            ( setOfTokens |> Seq.sumBy( scoreToken ))

    let classify ( groups : ( ClassifiedSMS * Group )[] )
                 ( textToClassify : string ) =
                
        let tokenizedText = tokenize textToClassify
        groups
        |> Array.maxBy( fun ( _, group ) ->
            score tokenizedText group )
        |> fst
        |> extractTypeFromRecord 

module Trainer = 

    let train = 23