open Constants
open Styles

module FeatureCard = {
  @react.component
  let make = (~feature, ~index) => {
    let mb = index != Array.length(features) - 1 ? "mb-6" : "mb-9"

    <div className={`${mb} flex flex-row p-6 rounded-[20px] feature-card`}>
      <div className={`${styles["flexCenter"]} w-[64px] h-[64px] rounded-full bg-dimBlue`}>
        <img src={feature["icon"]} alt="icon" className="w-[50%] h-[50%] object-contain" />
      </div>
      <div className="flex-1 flex flex-col ml-3">
        <h4 className="font-poppins font-semibold text-white text-[18px] leading-[23px] mb-1">
          {feature["title"]->React.string}
        </h4>
        <p className="font-poppins font-normal text-dimWhite text-[18px] leading-[23px] mb-1">
          {feature["content"]->React.string}
        </p>
      </div>
    </div>
  }
}

@react.component
let make = () => {
  <section>
    <div className={`${layout["sectionInfo"]}`}>
      <h2 className={`${styles["heading2"]}`}>
        {"You do the business, "->React.string}
        <br className="sm:block hidden" />
        {"we'll handle the money."->React.string}
      </h2>
      <p className={`${styles["paragraph"]} max-w-[470px] mt-5`}>
        {"With the right credit card, you can improve your financial life by building credit, earning rewards and saving money. But with hundreds of credit cards on the market."->React.string}
      </p>
      <Button styles="mt-10" />
    </div>
    <div className={`${layout["sectionImg"]} flex-col`}>
      {Array.mapWithIndex(features, (feature, index) => {
        <FeatureCard key={feature["id"]} feature index />
      })->React.array}
    </div>
  </section>
}
