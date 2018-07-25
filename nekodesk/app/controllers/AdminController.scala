package controllers

import javax.inject.Inject

import com.mohiva.play.silhouette.api.actions.SecuredRequest
import com.mohiva.play.silhouette.api.{LogoutEvent, Silhouette}
import org.webjars.play.WebJarsUtil
import play.api.i18n.I18nSupport
import play.api.mvc.{AbstractController, AnyContent, ControllerComponents}
import utils.auth.{DefaultEnv, WithRole}

import scala.concurrent.Future

/**
  * The basic application controller.
  *
  * @param components  The Play controller components.
  * @param silhouette  The Silhouette stack.
  * @param webJarsUtil The webjar util.
  * @param assets      The Play assets finder.
  */
class AdminController @Inject()(
                                 components: ControllerComponents,
                                 silhouette: Silhouette[DefaultEnv]
                               )(
                                 implicit
                                 webJarsUtil: WebJarsUtil,
                                 assets: AssetsFinder
                               ) extends AbstractController(components) with I18nSupport {

  def index = silhouette.SecuredAction(WithRole("neko")).async { implicit request: SecuredRequest[DefaultEnv, AnyContent] =>
    Future.successful(Ok(views.html.admin(request.identity)))
  }
}
