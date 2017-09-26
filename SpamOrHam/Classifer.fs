module Domain =

    type SMSType =
        | Spam
        | Ham

    type ClassifiedSMS = { Type : SMSType; Text : string }

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

    let parseAllLines : ClassifiedSMS seq = 
        File.ReadAllLines dataPath
        |> Seq.toList
        |> Seq.map parseSingleLine