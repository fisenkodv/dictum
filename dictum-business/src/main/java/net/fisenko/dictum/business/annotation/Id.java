package net.fisenko.dictum.business.annotation;

import net.fisenko.dictum.core.util.Strings;

import javax.validation.Constraint;
import javax.validation.ConstraintValidator;
import javax.validation.ConstraintValidatorContext;
import javax.validation.Payload;
import java.lang.annotation.Documented;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.util.Set;

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

        private final static Set<Character> HEX_CHARS = Set.of('0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                                               'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F');

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
                int length = hexString.length();
                if (length != 24) {
                    return false;
                } else {
                    for (int i = 0; i < length; ++i) {
                        if (!HEX_CHARS.contains(hexString.charAt(i))) {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }
    }
}
