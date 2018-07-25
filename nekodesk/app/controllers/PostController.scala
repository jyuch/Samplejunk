package controllers

import java.util.UUID
import javax.inject.Inject

import com.mohiva.play.silhouette.api.Silhouette
import com.mohiva.play.silhouette.api.actions.SecuredRequest
import forms.NewPostForm
import forms.NewPostForm.NewPost
import models.Post
import models.daos.PostDAO
import org.webjars.play.WebJarsUtil
import play.api.data.Form
import play.api.i18n.I18nSupport
import play.api.mvc._
import utils.auth.DefaultEnv

import scala.concurrent.ExecutionContext

class PostController @Inject()(
                                posts: PostDAO,
                                cc: MessagesControllerComponents,
                                components: ControllerComponents,
                                silhouette: Silhouette[DefaultEnv]
                              )(
                                implicit
                                webJarsUtil: WebJarsUtil,
                                assets: AssetsFinder,
                                ec: ExecutionContext
                              ) extends AbstractController(components) with I18nSupport {

  def list = silhouette.SecuredAction.async { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    posts.list.map { articles =>
      Ok(views.html.post.index(request.identity, articles))
    }
  }

  def detail(id: UUID) = silhouette.SecuredAction.async { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    posts.findById(id) map {
      case Some(a) => Ok(views.html.post.detail(request.identity, a))
      case None => Redirect(routes.PostController.list)
    }
  }

  def createNew = silhouette.SecuredAction { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    Ok(views.html.post.create(request.identity, NewPostForm.form))
  }

  def create = silhouette.SecuredAction { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    val errorFunction = { formWithErrors: Form[NewPost] =>
      BadRequest(views.html.post.create(request.identity, formWithErrors))
    }

    val successFunction = { article: NewPost =>
      val p = Post(UUID.randomUUID(), article.title, article.content)
      posts.save(p)
      Redirect(routes.PostController.list).flashing("info" -> "Article added!")
    }

    NewPostForm.form.bindFromRequest.fold(errorFunction, successFunction)
  }

  def delete(id:UUID) = silhouette.SecuredAction.async { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    posts.findById(id) map {
      case Some(a) => Ok(views.html.post.delete(request.identity, a))
      case None => Redirect(routes.PostController.list)
    }
  }
  def deleteConfirmed(id:UUID) = silhouette.SecuredAction.async { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    for(
      _ <- posts.deleteById(id)
    ) yield {
      Redirect(routes.PostController.list).flashing("info" -> "Article deleted!")
    }
  }
}
