open Constants
open Styles

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
      <div className="mt-10" />
      <Button />
    </div>
  </section>
}
