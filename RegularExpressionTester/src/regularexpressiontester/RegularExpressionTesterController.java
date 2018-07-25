package regularexpressiontester;

import java.net.URL;
import java.util.ResourceBundle;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.regex.PatternSyntaxException;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.scene.control.TreeItem;
import javafx.scene.control.TreeView;

/**
 *
 * @author jyuch
 * @since 1.0
 */
public class RegularExpressionTesterController implements Initializable {
    
    @FXML
    private TextField regularExpression;
    
    @FXML
    private TextArea targetString;
    
    @FXML
    private TextField replacement;
    
    @FXML
    private Label isMatch;
    
    @FXML
    private Label isFind;
    
    @FXML
    private Label groupCount;
    
    @FXML
    private Label replaceFirst;
    
    @FXML
    private Label replaceAll;
    
    @FXML
    private Label errorRegularExpression;
    
    @FXML
    private Label errorTargetString;
    
    @FXML
    private Label errorMessage;
    
    @FXML
    private TreeView<String> matcherFind;
    
    @FXML
    private CheckBox canonEq;
    
    @FXML
    private CheckBox caseInsensitive;
    
    @FXML
    private CheckBox comments;
    
    @FXML
    private CheckBox dotall;
    
    @FXML
    private CheckBox literal;
    
    @FXML
    private CheckBox multiline;
    
    @FXML
    private CheckBox unicodeCase;
    
    @FXML
    private CheckBox unicodeCharacterClass;
    
    @FXML
    private CheckBox unixLines;
    
    
    @FXML
    private void handleTestButtonAction(ActionEvent event) {
        errorRegularExpression.setVisible(false);
        errorTargetString.setVisible(false);
        errorMessage.setVisible(false);
        
        String re = regularExpression.getText();
        String target = targetString.getText();
        String replace = replacement.getText();
        
        if (re.isEmpty()) {
            errorRegularExpression.setText("Specify the field Regular Expression");
            errorRegularExpression.setVisible(true);
            return;
        } else if(target.isEmpty()) {
            errorTargetString.setVisible(true);
            return;
        }
        
        int compileFlags = buildCompileFlags();
        
        Pattern pa;
        try {
            pa = Pattern.compile(re, compileFlags);
        } catch (PatternSyntaxException ex) {
            errorRegularExpression.setText("Pattern compile failed");
            errorRegularExpression.setVisible(true);
            errorMessage.setText(ex.getMessage());
            errorMessage.setVisible(true);
            return;
        }
        
        Matcher ma = pa.matcher(target);
        
        if (ma.matches()) {
            isMatch.setText("true");
            isMatch.setStyle("-fx-text-fill: green;");
        } else {
            isMatch.setText("false");
            isMatch.setStyle("-fx-text-fill: red;");
        }
        
        ma.reset();
        if (ma.find()) {
            isFind.setText("true");
            isFind.setStyle("-fx-text-fill: green;");
        } else {
            isFind.setText("false");
            isFind.setStyle("-fx-text-fill: red;");
        }
        
        ma.reset();
        groupCount.setText(Integer.toString(ma.groupCount()));
        
        if (!replace.isEmpty()) {
            ma.reset();
            replaceFirst.setText(ma.replaceFirst(replace));
            ma.reset();
            replaceAll.setText(ma.replaceAll(replace));
        } else {
            replaceFirst.setText("");
            replaceAll.setText("");
        }
        
        ma.reset();
        TreeItem<String> root = new TreeItem("find()");
        root.setExpanded(true);
        int findCount = 0;
        while(ma.find()) {
            findCount++;
            TreeItem<String> subRoot = new TreeItem<>(String.format("#%d : \"%s\"", findCount, ma.group()));
            for(int i = 0; i <= ma.groupCount(); i++) {
                TreeItem<String> e = new TreeItem<>(String.format("group(%d) : \"%s\"", i, ma.group(i)));
                subRoot.getChildren().add(e);
            }
            root.getChildren().add(subRoot);
        }
        
        matcherFind.setRoot(root);
    }

    /**
     * チェックボックスからパターンへのコンパイル時に使用するフラグを構築します。
     * 
     * @return コンパイルフラグ
     */
    private int buildCompileFlags() {
        int compileFlags = 0;
        if(canonEq.isSelected()) {
            compileFlags = compileFlags | Pattern.CANON_EQ;
        }
        if (caseInsensitive.isSelected()) {
            compileFlags = compileFlags | Pattern.CASE_INSENSITIVE;
        }
        if (comments.isSelected()) {
            compileFlags = compileFlags | Pattern.COMMENTS;
        }
        if (dotall.isSelected()) {
            compileFlags = compileFlags | Pattern.DOTALL;
        }
        if (literal.isSelected()) {
            compileFlags = compileFlags | Pattern.LITERAL;
        }
        if (multiline.isSelected()) {
            compileFlags = compileFlags | Pattern.MULTILINE;
        }
        if (unicodeCase.isSelected()) {
            compileFlags = compileFlags | Pattern.UNICODE_CASE;
        }
        if (unicodeCharacterClass.isSelected()) {
            compileFlags = compileFlags | Pattern.UNICODE_CHARACTER_CLASS;
        }
        if (unixLines.isSelected()) {
            compileFlags = compileFlags | Pattern.UNIX_LINES;
        }
        return compileFlags;
    }
    
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        // TODO
    }    
    
}
