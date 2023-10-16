@react.component
let make = (~styles) => {
  <button
    className={`${styles} py-4 px-6 font-poppins font-medium text-[18px] text-primary bg-blue-gradient rounded-[10px] outline-none`}>
    {"Get Started"->React.string}
  </button>
}
