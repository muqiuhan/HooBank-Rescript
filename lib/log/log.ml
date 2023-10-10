module Stdout_Log = Dog.Make (Dog.Builtin.Logger)

module File_log = struct
  module Printer = struct
    let path : string ref = ref "suka.log"
  end

  include Dog.Make (struct
    include Dog.Filter.Builtin
    include Dog.Formatter.Builtin
    include Dog.Recorder.Builtin

    module Printer = Dog.Printer.Builtin.File_Printer (struct
      let path = !Printer.path
    end)
  end)
end

module Log = struct
  let[@inline always] __record ~str ~level ~file =
    match file with
    | None -> Stdout_Log.__record ~str ~level
    | Some file ->
      File_log.Printer.path := file;
      File_log.__record ~str ~level

  let[@inline always] info ?(file : string option = None) (fmt : 'a) =
    Format.ksprintf
      (fun str -> __record ~file ~str ~level:Dog.Recorder.Level.Info)
      fmt

  let[@inline always] error ?(file : string option = None) (fmt : 'a) =
    Format.ksprintf
      (fun str -> __record ~file ~str ~level:Dog.Recorder.Level.Error)
      fmt

  let[@inline always] warn ?(file : string option = None) (fmt : 'a) =
    Format.ksprintf
      (fun str -> __record ~file ~str ~level:Dog.Recorder.Level.Warn)
      fmt

  let[@inline always] debug ?(file : string option = None) (fmt : 'a) =
    Format.ksprintf
      (fun str -> __record ~file ~str ~level:Dog.Recorder.Level.Debug)
      fmt
end
