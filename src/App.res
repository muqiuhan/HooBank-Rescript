@module("./assets/logo.svg") external logo: string = "default"

open Styles

@react.component
let make = () => {
  <div className="bg-primary w-full overflow-hidden">
    <div className={`${styles["paddingX"]} ${styles["flexCenter"]}`}>
      <Navbar />
    </div>
    <div className={`bg-primary ${styles["flexStart"]}`}>
      <div className={`${styles["boxWidth"]}`}>
        <Hero />
      </div>
    </div>
    <div className={`bg-primary ${styles["paddingX"]} ${styles["flexStart"]}`}>
      <div className={`${styles["boxWidth"]}`}>
        <Stats />
        <Business />
        <Billing />
        <CardDeal />
        <Testimonials />
        <Client />
        <CTA />
        <Footer />
      </div>
    </div>
  </div>
}
