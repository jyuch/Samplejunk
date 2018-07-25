package forms

object NewPostForm {
  import play.api.data.Forms._
  import play.api.data.Form

  case class NewPost(title: String, content: String)

  val form = Form(
    mapping(
      "title" -> nonEmptyText(maxLength = 100, minLength = 1),
      "content" -> text
    )(NewPost.apply)(NewPost.unapply)
  )
}
