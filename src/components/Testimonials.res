open Styles
open Constants

@react.component
let make = () => {
  <section
    id="clients" className={`${styles["paddingY"]} ${styles["flexCenter"]} flex-col relative `}>
    <div
      className="absolute z-[0] w-[60%] h-[60%] blue__gradient bottom-40"
    />
    <div
      className="absolute z-[0] w-[60%] h-[60%] -right-[50%] rounded-full blue__gradient bottom-40"
    />
    <div
      className="w-full flex justify-between items-center md:flex-row flex-col sm:mb-16 mb-6 relative z-[1]">
      <h2 className={`${styles["heading2"]}`}>
        {"What People are"->React.string}
        <br className="sm:block hidden" />
        {"saying about us"->React.string}
      </h2>
      <div className="w-full md:mt-0 mt-6">
        <p className={`${styles["paragraph"]} text-left max-w-[450px]`}>
          {"Everything you need to accept card payments and grow your business
          anywhere on the planet."->React.string}
        </p>
      </div>
    </div>
    <div
      className="flex flex-wrap sm:justify-start justify-center w-full feedback-container relative z-[1]">
      {Array.map(feedback, card => <FeedbackCard key={card["id"]} card />)->React.array}
    </div>
  </section>
}
