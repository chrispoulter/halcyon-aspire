'use client';

import { GetUserResponse } from '@/app/actions/getUserAction';
import { deleteUserAction } from '@/app/actions/deleteUserAction';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Button } from '@/components/ui/button';
import { toast } from '@/hooks/use-toast';

type DeleteUserButtonProps = {
    user: GetUserResponse;
    className?: string;
};

export function DeleteUserButton({ user, className }: DeleteUserButtonProps) {
    async function onDelete() {
        const result = await deleteUserAction({ id: user.id });
        console.log('result', result);

        toast({
            title: 'User successfully deleted.',
        });
    }
    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <Button variant="destructive" className={className}>
                    Delete
                </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Delete User</AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to delete this user account? All
                        of the data will be permanently removed. This action
                        cannot be undone.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction onClick={onDelete}>
                        Delete
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
