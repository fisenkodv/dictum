package net.fisenko.dictum.core.annotation;

import net.fisenko.dictum.core.util.Strings;

import javax.validation.Constraint;
import javax.validation.ConstraintValidator;
import javax.validation.ConstraintValidatorContext;
import javax.validation.Payload;
import java.lang.annotation.Documented;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

import static java.lang.annotation.ElementType.FIELD;
import static java.lang.annotation.ElementType.METHOD;
import static java.lang.annotation.ElementType.PARAMETER;
import static java.lang.annotation.ElementType.TYPE_USE;

@Documented
@Constraint(validatedBy = Id.ObjectIdValidator.class)
@Target({METHOD, FIELD, PARAMETER, TYPE_USE})
@Retention(RetentionPolicy.RUNTIME)
public @interface Id {

    boolean canBeEmpty() default false;

    String message() default "is invalid identifier";

    Class<?>[] groups() default {};

    Class<? extends Payload>[] payload() default {};

    class ObjectIdValidator implements ConstraintValidator<Id, String> {

        private Id annotation;

        @Override
        public void initialize(Id annotation) {
            this.annotation = annotation;
        }

        @Override
        public boolean isValid(String objectId, ConstraintValidatorContext constraintValidatorContext) {
            return Strings.isNullOrEmpty(objectId) ? annotation.canBeEmpty() : isValid(objectId);
        }

        private boolean isValid(String hexString) {
            if (hexString == null) {
                throw new IllegalArgumentException();
            } else {
                int len = hexString.length();
                if (len != 24) {
                    return false;
                } else {
                    for (int i = 0; i < len; ++i) {
                        char c = hexString.charAt(i);
                        if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F')) {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }
    }
}
