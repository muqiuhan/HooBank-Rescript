// Generated by ReScript, PLEASE EDIT WITH CARE

import * as Styles from "../Styles.bs.js";
import * as JsxRuntime from "react/jsx-runtime";
import BillPng from "../assets/bill.png";
import AppleSvg from "../assets/apple.svg";
import GoogleSvg from "../assets/google.svg";

var bill_image = BillPng;

var apple_logo = AppleSvg;

var google_logo = GoogleSvg;

function Billing(props) {
  return JsxRuntime.jsxs("section", {
              children: [
                JsxRuntime.jsxs("div", {
                      children: [
                        JsxRuntime.jsx("img", {
                              className: "w-[100%] h-[100%] relative z-[5]",
                              alt: "billing",
                              src: bill_image
                            }),
                        JsxRuntime.jsx("div", {
                              className: "absolute z-[3] -left-1/2 top-0 w-[50%] h-[50%] rounded-full white__gradient"
                            }),
                        JsxRuntime.jsx("div", {
                              className: "absolute z-[0] w-[50%] h-[50%] -left-1/2 bottom-0 rounded-full pink__gradient"
                            })
                      ],
                      className: "" + Styles.layout.sectionImgReverse + ""
                    }),
                JsxRuntime.jsxs("div", {
                      children: [
                        JsxRuntime.jsxs("h2", {
                              children: [
                                "Easily control your",
                                JsxRuntime.jsx("br", {
                                      className: "sm:block hidden"
                                    }),
                                "billing & invoicing"
                              ],
                              className: "" + Styles.styles.heading2 + ""
                            }),
                        JsxRuntime.jsx("p", {
                              children: "Elit enim sed massa etiam. Mauris eu adipiscing ultrices ametodio\n        aenean neque. Fusce ipsum orci rhoncus aliporttitor integer platea\n        placerat.",
                              className: "" + Styles.styles.paragraph + " max-w-[470px] mt-5"
                            }),
                        JsxRuntime.jsxs("div", {
                              children: [
                                JsxRuntime.jsx("img", {
                                      className: "w-[128.86px] h-[42.05px] object-contain mr-5 cursor-pointer",
                                      alt: "google_play",
                                      src: apple_logo
                                    }),
                                JsxRuntime.jsx("img", {
                                      className: "w-[144.17px] h-[43.08px] object-contain cursor-pointer",
                                      alt: "google_play",
                                      src: google_logo
                                    })
                              ],
                              className: "flex flex-row flex-wrap sm:mt-10 mt-6"
                            })
                      ],
                      className: "" + Styles.layout.sectionInfo + ""
                    })
              ],
              className: "" + Styles.layout.sectionReverse + "",
              id: "product"
            });
}

var make = Billing;

export {
  bill_image ,
  apple_logo ,
  google_logo ,
  make ,
}
/* bill_image Not a pure module */
