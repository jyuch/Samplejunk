package forms

import play.api.data.Form
import play.api.data.Forms._

/**
 * The form which handles the sign up process.
 */
object SignUpForm {

  /**
   * A play framework form.
   */
  val form = Form(
    mapping(
      "fullName" -> nonEmptyText,
      "loginId" -> nonEmptyText,
      "password" -> nonEmptyText
    )(Data.apply)(Data.unapply)
  )

  case class Data(
    fullName: String,
    loginId: String,
    password: String)
}
