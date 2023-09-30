open System
open System.IO

let separator ="\t"
let culture = System.Globalization.CultureInfo.InvariantCulture

let generate fileName sizeMB=
    let size = sizeMB * 1024 * 1024
    File.Delete(fileName)
    use output = File.OpenWrite(fileName)
    use writer = new StreamWriter(output)

    let write =
        String.concat separator
        >> writer.WriteLine
    
    write [| "ID"; "Name"; "DateOfBirth"; "Preference"; "Something"; "Money" |]
    
    let f = Bogus.Faker()
    let mutable id = (int64)0
    while (output.Length < size) do
        id <- id + (int64)1
        write [|
            id.ToString(culture)
            f.Name.FullName()
            (f.Date.Past 100).ToString(culture)
            f.Company.CompanyName()
            f.Random.Int(0,100).ToString()
            f.Random.Decimal((decimal)0, (decimal)999999999).ToString("n2", culture)
        |]
        
let usage() =
    Console.WriteLine("Usage: generate <destination.csv> [size in MB]")

[<EntryPoint>]
let main args =
    match args with
    | [| file |] -> generate file 5
    | [| file; mb |] ->
        match Int32.TryParse mb with
        | true, s -> generate file s
        | _ -> usage()
    | _ -> usage()
    
    0 // return an integer exit code
        