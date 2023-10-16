@react.component
let make = (~styles) => {
  <span className=styles>
    <button
      className={`py-4 px-6 font-poppins font-medium text-[18px] text-primary bg-blue-gradient rounded-[10px] outline-none hover:opacity-80`}>
      {"Get Started"->React.string}
    </button>
  </span>
}
